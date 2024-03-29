global using System.ComponentModel.DataAnnotations;
global using System.Globalization;
global using System.Net;
global using System.Security.Cryptography.X509Certificates;
global using System.Text;
global using System.Text.Encodings.Web;
global using System.Text.Json;
global using Ardalis.GuardClauses;
global using CmsEngine.Application.Helpers;
global using CmsEngine.Application.Helpers.Email;
global using CmsEngine.Application.Models.EditModels;
global using CmsEngine.Application.Models.ViewModels;
global using CmsEngine.Application.Models.ViewModels.DataTablesViewModels;
global using CmsEngine.Application.Services;
global using CmsEngine.Application.Services.Interfaces;
global using CmsEngine.Core.Constants;
global using CmsEngine.Core.Extensions;
global using CmsEngine.Core.Utils;
global using CmsEngine.Data;
global using CmsEngine.Data.Entities;
global using CmsEngine.Data.Repositories;
global using CmsEngine.Data.Repositories.Interfaces;
global using CmsEngine.Ui.Areas.Cms.Controllers;
global using CmsEngine.Ui.Extensions;
global using CmsEngine.Ui.Middleware;
global using CmsEngine.Ui.Middleware.SecurityHeaders;
global using CmsEngine.Ui.RewriteRules;
global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Http.Extensions;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.UI.Services;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.AspNetCore.Mvc.RazorPages;
global using Microsoft.AspNetCore.Mvc.Rendering;
global using Microsoft.AspNetCore.Razor.TagHelpers;
global using Microsoft.AspNetCore.Rewrite;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.DependencyInjection.Extensions;
global using Microsoft.Extensions.FileProviders;
global using Microsoft.Net.Http.Headers;
global using Serilog;
global using SameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode;
global using ILogger = Microsoft.Extensions.Logging.ILogger;
