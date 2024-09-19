// System
global using System.Reflection;
global using System.Security.Claims;



// Microsoft
global using Microsoft.AspNetCore.Mvc.ApiExplorer;
global using Microsoft.Extensions.Options;
global using Microsoft.OpenApi.Models;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Versioning;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.AspNetCore.SignalR;
global using Microsoft.AspNetCore.Authorization;



// Third-party
global using Swashbuckle.AspNetCore.SwaggerGen;
global using AutoMapper;
global using Swashbuckle.AspNetCore.Annotations;


// BugNetCore
// BugNetCore.Api
global using BugNetCore.Api.Swagger;
global using BugNetCore.Api.Swagger.Models;
global using BugNetCore.Api.ApiVersionSupport;
global using customWebExceptions = BugNetCore.Api.Exceptions;
global using BugNetCore.Api.Controllers.V1._0_Beta.Base;
global using BugNetCore.Api.Filters.ExceptionFilters;





// BugNetCore.Services
global using BugNetCore.Services.Logging.Configuration;
global using BugNetCore.Services.Logging.Interfaces;
global using BugNetCore.Services.DataServices.Helpers;
global using BugNetCore.Services.DataServices.Interfaces;



// BugNetCore.Dal
global using BugNetCore.Dal.EFStructures;
global using BugNetCore.Dal.Initialization;
global using BugNetCore.Dal.Exceptions;
global using BugNetCore.Dal.Repos.Base;
global using BugNetCore.Dal.Repos.Interfaces;


// BugNetCore.Models
global using BugNetCore.Models.Entities;
global using BugNetCore.Models.Entities.Base;
global using BugNetCore.Models.DTOs.Base;
global using BugNetCore.Models.DTOs.Bug;
global using BugNetCore.Models.DTOs.Comment;
global using BugNetCore.Models.DTOs.Project;
global using BugNetCore.Models.DTOs.SupportRequest;
global using BugNetCore.Models.DTOs.User;
global using BugNetCore.Models.DTOs.ChatMessage;
global using BugNetCore.Models.DTOs.Notification;





