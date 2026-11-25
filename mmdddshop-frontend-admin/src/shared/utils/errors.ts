/**
 * Extracts a readable message from a value caught in a `catch` block.
 * Anything can be thrown in JavaScript, so the caught value is `unknown`
 * and has to be narrowed before `.message` can be read.
 */
export const getErrorMessage = (error: unknown, fallback: string): string => {
    if (
        typeof error === 'object' &&
        error !== null &&
        'message' in error &&
        typeof error.message === 'string' &&
        error.message
    ) {
        return error.message;
    }
    return fallback;
};
