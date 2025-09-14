// Global using statements for the UserAccess Application module
// These imports are available throughout the entire module without explicit using statements

// MediatR for CQRS pattern implementation
global using MediatR;

// Application contracts and interfaces
global using ModularMonolithDDD.Modules.UserAccess.Application.Contracts;

// Dapper for data access
global using Dapper;

// Building blocks for data access
global using ModularMonolithDDD.BuildingBlocks.Application.Data;

// Query and command configuration
global using ModularMonolithDDD.Modules.UserAccess.Application.Configuration.Queries;
global using ModularMonolithDDD.Modules.UserAccess.Application.Configuration.Commands;

global using System.Runtime.CompilerServices;
global using System.Security.Cryptography;

global using ModularMonolithDDD.Modules.UserAccess.Application.Authentication.Authenticate;
global using ModularMonolithDDD.Modules.UserAccess.Domain.Users;