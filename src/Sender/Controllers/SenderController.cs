using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Security.Cryptor.Contracts;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Sender.Models;
using Sender.Services.Contracts;
using Core.ZipManagementService.Contracts;

namespace Sender.Controllers
{
    public class SenderController : Controller
    {
        private readonly IFileManagementService _fileManagementService;

        private readonly IZipManagementService _zipManagementService;

        private readonly IEncrypter _encrypter;

        private readonly ITransferService _transferService;

        public SenderController(IFileManagementService fileManagementService,
            IZipManagementService zipManagementService,
            IEncrypter encrypter, ITransferService transferService)
        {
            this._transferService = transferService ?? throw new ArgumentNullException(nameof(transferService));
            this._encrypter = encrypter ?? throw new ArgumentNullException(nameof(encrypter));
            this._fileManagementService =
                fileManagementService ?? throw new ArgumentNullException(nameof(fileManagementService));
            this._zipManagementService =
                zipManagementService ?? throw new ArgumentNullException(nameof(zipManagementService));
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UploadViewModel model)
        {
            if (model == null)
                return BadRequest();

            if (ModelState.IsValid)
            {
                var savedResult = await _fileManagementService.SaveZipFileToLocalFolder(model.File);

                if (savedResult.result)
                {
                    var result = await _zipManagementService.GetSerializedDirectoryStructure(savedResult.savedFilePath);

                    // var encryptedData = await _encrypter.EncryptAsync(result);

                    var response = await _transferService.PostToRecipientAsync("Post",
                        new TransferModel()
                        {
                            UserName = await _encrypter.EncryptAsync(model.UserName),
                            Password = await _encrypter.EncryptAsync(model.Password),
                            JsonData = result
                        });

                    if (response == null)
                        return View("Result", new ResultViewModel()
                        {
                            Errors = new List<ErrorModel>()
                            {
                                new ErrorModel()
                                {
                                    ErrorMessage = "Authentication Error : Invalid username or password"
                                }
                            }
                        });
                    return View("Result", response);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Result(ResultViewModel model)
        {
            return View(model);
        }
    }
}