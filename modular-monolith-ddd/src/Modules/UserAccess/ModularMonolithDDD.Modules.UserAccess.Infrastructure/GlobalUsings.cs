// Global using statements for the UserAccess Infrastructure module
// These statements make the specified types available throughout the entire module
// without requiring explicit using statements in each file

// Autofac dependency injection container
global using Autofac;
global using Autofac.Core.Activators.Reflection;
global using FluentValidation;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
global using Microsoft.Extensions.Logging;
global using ModularMonolithDDD.BuildingBlocks.Application.Data;
global using ModularMonolithDDD.BuildingBlocks.Application.Outbox;
global using ModularMonolithDDD.BuildingBlocks.Infrastructure;
global using ModularMonolithDDD.BuildingBlocks.Infrastructure.InternalCommands;
global using ModularMonolithDDD.Modules.UserAccess.Domain.Users;
// Import the configuration namespace to access UserAccessCompositionRoot
global using ModularMonolithDDD.Modules.UserAccess.Infrastructure.Domain.Users;
global using ModularMonolithDDD.Modules.UserAccess.Infrastructure.InternalCommands;
global using ModularMonolithDDD.Modules.UserAccess.Infrastructure.Outbox;
global using System.Collections.Concurrent;
global using System.Reflection;
// Serilog logging interface - aliased to avoid conflicts with other logging frameworks
// Autofac module base class - aliased for clarity and to avoid naming conflicts
global using Module = Autofac.Module;
