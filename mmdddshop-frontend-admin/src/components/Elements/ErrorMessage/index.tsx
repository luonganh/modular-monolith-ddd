import { FieldError, FieldErrorsImpl, FieldValues, Merge } from 'react-hook-form';

interface ErrorMessageInterface {
    message: string | FieldError | Merge<FieldError, FieldErrorsImpl<FieldValues>>;
}

const ErrorMessage: React.FC<ErrorMessageInterface> = ({ message }) => (
    <p
        role="alert"
        className="text-sm text-light-error500 dark:text-dark-error500"
    >
        {message.toString()}
    </p>
);

export default ErrorMessage;
