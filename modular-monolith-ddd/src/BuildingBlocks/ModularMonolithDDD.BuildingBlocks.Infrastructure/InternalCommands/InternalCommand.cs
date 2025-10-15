namespace ModularMonolithDDD.BuildingBlocks.Infrastructure.InternalCommands
{
    /// <summary>
    /// Represents an internal command that is stored in the database for background processing.
    /// Internal commands are used to defer the execution of certain operations that need to be
    /// processed asynchronously or in a different context within the modular monolith architecture.
    /// </summary>
    public class InternalCommand
    {
        /// <summary>
        /// Gets or sets the unique identifier of the internal command.
        /// This ID is used for tracking, deduplication, and correlation purposes
        /// to ensure each command is processed only once.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the full type name of the internal command.
        /// This is used for deserialization and routing the command to the
        /// appropriate handler when processing.
        /// </summary>
        public string Type { get; set; } = default!;

        /// <summary>
        /// Gets or sets the serialized JSON data of the internal command.
        /// This contains the complete command payload that will be deserialized
        /// and processed by the appropriate handler.
        /// </summary>
        public string Data { get; set; } = default!;

        /// <summary>
        /// Gets or sets the timestamp when the command was successfully processed.
        /// This is null for unprocessed commands and set to the processing time
        /// when the command has been handled to prevent duplicate processing.
        /// </summary>
        public DateTime? ProcessedDate { get; set; }
    }
}