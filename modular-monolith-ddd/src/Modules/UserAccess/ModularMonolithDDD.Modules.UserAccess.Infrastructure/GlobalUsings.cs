// Global using statements for the UserAccess Infrastructure module
// These statements make the specified types available throughout the entire module
// without requiring explicit using statements in each file

// Autofac dependency injection container
global using Autofac;
global using Autofac.Core;
global using Autofac.Core.Activators.Reflection;
global using Autofac.Features.Variance;
global using Dapper;
global using FluentValidation;
global using MediatR;
global using MediatR.Pipeline;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
global using Microsoft.Extensions.Logging;
global using ModularMonolithDDD.BuildingBlocks.Application;
global using ModularMonolithDDD.BuildingBlocks.Application.Data;
global using ModularMonolithDDD.BuildingBlocks.Application.Emails;
global using ModularMonolithDDD.BuildingBlocks.Application.Events;
global using ModularMonolithDDD.BuildingBlocks.Application.Outbox;
global using ModularMonolithDDD.BuildingBlocks.Infrastructure;
global using ModularMonolithDDD.BuildingBlocks.Infrastructure.DomainEventsDispatching;
global using ModularMonolithDDD.BuildingBlocks.Infrastructure.Emails;
global using ModularMonolithDDD.BuildingBlocks.Infrastructure.EventBus;
global using ModularMonolithDDD.BuildingBlocks.Infrastructure.InternalCommands;
global using ModularMonolithDDD.BuildingBlocks.Infrastructure.Serialization;
global using ModularMonolithDDD.Modules.UserAccess.Application.Configuration.Commands;
global using ModularMonolithDDD.Modules.UserAccess.Application.Contracts;
global using ModularMonolithDDD.Modules.UserAccess.Domain.Users;
// Import the configuration namespace to access UserAccessCompositionRoot
global using ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration;
global using ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.DataAccess;
global using ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.EventsBus;
global using ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Logging;
global using ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Mediation;
global using ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Processing;
global using ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Processing.Inbox;
global using ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Processing.InternalCommands;
global using ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Processing.Outbox;
global using ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Quartz;
global using ModularMonolithDDD.Modules.UserAccess.Infrastructure.Domain.Users;
global using ModularMonolithDDD.Modules.UserAccess.Infrastructure.InternalCommands;
global using ModularMonolithDDD.Modules.UserAccess.Infrastructure.Outbox;
global using Newtonsoft.Json;
global using Polly;
global using Quartz;
global using Quartz.Impl;
global using Quartz.Logging;
global using Serilog.Core;
global using Serilog.Events;
global using System.Collections.Concurrent;
global using System.Collections.Immutable;
global using System.Collections.Specialized;
global using System.Reflection;
global using System.Security.Cryptography;
global using System.Text;
// Serilog logging interface - aliased to avoid conflicts with other logging frameworks
global using ILogger = Serilog.ILogger;
global using Logger = Quartz.Logging.Logger;
global using LogLevel = Quartz.Logging.LogLevel;
// Autofac module base class - aliased for clarity and to avoid naming conflicts
global using Module = Autofac.Module;
global using TriggerBuilder = Quartz.TriggerBuilder;
global using OpenIddict.Abstractions;