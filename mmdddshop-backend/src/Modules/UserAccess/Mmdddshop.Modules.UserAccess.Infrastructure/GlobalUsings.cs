// Global using statements for the UserAccess Infrastructure module
// These statements make the specified types available throughout the entire module
// without requiring explicit using statements in each file

// Autofac dependency injection container
global using Newtonsoft.Json;
global using Polly;
global using Serilog.Core;
global using Serilog.Events;
global using System.Collections.Concurrent;
global using System.Reflection;
global using Autofac;
global using Autofac.Core;
global using Autofac.Core.Activators.Reflection;
global using Autofac.Features.Variance;
global using Dapper;
global using FluentValidation;
global using MediatR;
global using MediatR.Pipeline;
global using Quartz;
global using Quartz.Impl;
global using Quartz.Logging;
global using TriggerBuilder = Quartz.TriggerBuilder;
global using System.Collections.Specialized;
global using System.Security.Cryptography;
global using System.Text;

global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
global using Microsoft.Extensions.Logging;
global using Mmdddshop.BuildingBlocks.Application;
global using Mmdddshop.BuildingBlocks.Application.Data;
global using Mmdddshop.BuildingBlocks.Application.Events;
global using Mmdddshop.BuildingBlocks.Application.Outbox;
global using Mmdddshop.BuildingBlocks.Infrastructure;
global using Mmdddshop.BuildingBlocks.Infrastructure.DomainEventsDispatching;
global using Mmdddshop.BuildingBlocks.Application.Emails;
global using Mmdddshop.BuildingBlocks.Infrastructure.Emails;

global using Mmdddshop.BuildingBlocks.Infrastructure.InternalCommands;
global using Mmdddshop.BuildingBlocks.Infrastructure.Serialization;
global using Mmdddshop.BuildingBlocks.Infrastructure.EventBus;

global using Mmdddshop.Modules.UserAccess.Application.Configuration.Commands;
global using Mmdddshop.Modules.UserAccess.Application.Contracts;
global using Mmdddshop.Modules.UserAccess.Domain.Users;

// Import the configuration namespace to access UserAccessCompositionRoot
global using Mmdddshop.Modules.UserAccess.Infrastructure.Configuration.Processing.InternalCommands;
global using Mmdddshop.Modules.UserAccess.Infrastructure.Domain.Users;
global using Mmdddshop.Modules.UserAccess.Infrastructure.InternalCommands;
global using Mmdddshop.Modules.UserAccess.Infrastructure.Outbox;
global using Mmdddshop.Modules.UserAccess.Infrastructure.Configuration.Processing.Outbox;
global using Mmdddshop.Modules.UserAccess.Infrastructure.Configuration.Processing.Inbox;
global using Mmdddshop.Modules.UserAccess.Infrastructure.Configuration.DataAccess;
global using Mmdddshop.Modules.UserAccess.Infrastructure.Configuration.EventsBus;
global using Mmdddshop.Modules.UserAccess.Infrastructure.Configuration.Logging;
global using Mmdddshop.Modules.UserAccess.Infrastructure.Configuration.Mediation;
global using Mmdddshop.Modules.UserAccess.Infrastructure.Configuration.Processing;
global using Mmdddshop.Modules.UserAccess.Infrastructure.Configuration.Quartz;
global using Mmdddshop.Modules.UserAccess.Infrastructure.Configuration;
global using Mmdddshop.Modules.UserAccess.Infrastructure.Configuration.Email;
global using Mmdddshop.Modules.UserAccess.Infrastructure.Configuration.Security;

// Serilog logging interface - aliased to avoid conflicts with other logging frameworks
global using ILogger = Serilog.ILogger;
global using Logger = Quartz.Logging.Logger;
global using LogLevel = Quartz.Logging.LogLevel;
// Autofac module base class - aliased for clarity and to avoid naming conflicts
global using Module = Autofac.Module;