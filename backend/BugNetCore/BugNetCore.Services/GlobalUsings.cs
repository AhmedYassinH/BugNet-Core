
// System
global using System.Runtime.CompilerServices;
global using System.Security.Claims;



// Microsoft
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.Extensions.Configuration;
global using Microsoft.AspNetCore.Http;


// Third-party
global using Serilog.Context;
global using Serilog.Events;
global using AutoMapper;


// BugNetCore
// BugNetCore.Services
global using BugNetCore.Services.DataServices.Interfaces;
global using BugNetCore.Services.Logging.Interfaces;
global using BugNetCore.Services.Logging.Settings;
global using BugNetCore.Services.DataServices.Dal.Base;
global using BugNetCore.Services.DataServices.Dal;
global using BugNetCore.Services.DataServices.Helpers;
global using BugNetCore.Services.DataServices.Settings;




// BugNetCore.Dal
global using BugNetCore.Dal.Repos.Base;
global using BugNetCore.Dal.Repos;
global using BugNetCore.Dal.Repos.Interfaces;

// BugNetCore.Models
global using BugNetCore.Models.Entities.Base;
global using BugNetCore.Models.Entities;
global using BugNetCore.Models.DTOs.SupportRequest;
global using BugNetCore.Models.DTOs.User.Auth;
global using BugNetCore.Models.DTOs.Notification;
