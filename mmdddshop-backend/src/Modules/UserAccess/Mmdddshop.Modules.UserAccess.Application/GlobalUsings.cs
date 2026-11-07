// Global using statements for the UserAccess Application module
// These imports are available throughout the entire module without explicit using statements
global using MediatR;
global using Mmdddshop.Modules.UserAccess.Application.Contracts;
global using Dapper;
global using Mmdddshop.BuildingBlocks.Application.Data;
global using Mmdddshop.Modules.UserAccess.Application.Configuration.Commands;
global using Mmdddshop.Modules.UserAccess.Application.Configuration.Queries;
global using System.Security.Claims;
global using System.Runtime.CompilerServices;
global using System.Security.Cryptography;
global using Mmdddshop.Modules.UserAccess.Application.Authentication.Authenticate;
global using Mmdddshop.Modules.UserAccess.Domain.Users;
global using Mmdddshop.Modules.UserAccess.Application.Authentication;
global using FluentValidation;
global using System.Text.RegularExpressions;
global using Mmdddshop.Modules.UserAccess.Application.Users.GetUser;
global using Mmdddshop.BuildingBlocks.Application;
global using Mmdddshop.Modules.UserAccess.Application.Authentication;
global using Mmdddshop.Modules.UserAccess.Application.Users.GetUser;