// Global using statements for the UserAccess Infrastructure module
// These statements make the specified types available throughout the entire module
// without requiring explicit using statements in each file

// Autofac dependency injection container
global using Autofac;

// Serilog logging interface - aliased to avoid conflicts with other logging frameworks
global using ILogger = Serilog.ILogger;

// Autofac module base class - aliased for clarity and to avoid naming conflicts
global using Module = Autofac.Module;

global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
global using Microsoft.Extensions.Logging;
global using ModularMonolithDDD.BuildingBlocks.Application.Data;
global using ModularMonolithDDD.BuildingBlocks.Infrastructure;

global using ModularMonolithDDD.Modules.UserAccess.Domain.Users;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using ModularMonolithDDD.Modules.UserAccess.Infrastructure.Domain.Users;

global using Autofac.Core.Activators.Reflection;
global using System.Collections.Concurrent;
global using System.Reflection;

global using ModularMonolithDDD.BuildingBlocks.Application.Outbox;
global using ModularMonolithDDD.BuildingBlocks.Infrastructure.InternalCommands;
global using ModularMonolithDDD.Modules.UserAccess.Infrastructure.InternalCommands;
global using ModularMonolithDDD.Modules.UserAccess.Infrastructure.Outbox;

global using MediatR;
global using ModularMonolithDDD.BuildingBlocks.Application.Events;
global using ModularMonolithDDD.BuildingBlocks.Infrastructure.DomainEventsDispatching;
global using ModularMonolithDDD.Modules.UserAccess.Application.Configuration.Commands;
global using ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Processing.InternalCommands;
global using ModularMonolithDDD.Modules.UserAccess.Application.Contracts;

global using Quartz;
global using Dapper;
global using ModularMonolithDDD.BuildingBlocks.Infrastructure.Serialization;
global using Newtonsoft.Json;

global using ModularMonolithDDD.BuildingBlocks.Application;
global using Serilog.Core;
global using Serilog.Events;

global using FluentValidation;
global using Polly;

global using ModularMonolithDDD.BuildingBlocks.Infrastructure.EventBus;
