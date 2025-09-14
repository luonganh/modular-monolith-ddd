// Global using statements for the UserAccess Infrastructure module
// These statements make the specified types available throughout the entire module
// without requiring explicit using statements in each file

// Autofac dependency injection container
global using Autofac;

// Serilog logging interface - aliased to avoid conflicts with other logging frameworks
global using ILogger = Serilog.ILogger;

// Autofac module base class - aliased for clarity and to avoid naming conflicts
global using Module = Autofac.Module;
