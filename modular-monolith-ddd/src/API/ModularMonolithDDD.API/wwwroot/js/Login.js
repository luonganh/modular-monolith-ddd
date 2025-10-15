initLoginValidation();

function initLoginValidation() {    
    $.validator.addMethod('usernameOrEmail', function (v, el) {
    v = v.trim();
    const isUser = /^[A-Za-z0-9._-]{3,50}$/.test(v);
    const isEmail = this.optional(el) ? true : ($.validator.methods.email.call(this, v, el) && v.length <= 254);
    return isUser || isEmail;
    }, 'Enter a valid username or email.');
    
    $.validator.addMethod('strongPassword', function(value, element) {
        if (!value) return true; // Let required handle empty values
        
        const hasUpper = /[A-Z]/.test(value);           // At least 1 uppercase
        const hasLower = /[a-z]/.test(value);           // At least 1 lowercase  
        const hasDigit = /[0-9]/.test(value);           // At least 1 digit
        const hasSpecial = /[!@#$%^&*()_+\-=\[\]{}|;:,.<>?]/.test(value); // At least 1 special char
        
        return hasUpper && hasLower && hasDigit && hasSpecial;
    }, 'Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.');

    $.validator.addMethod('noEdgeSpace', function(value, element) {
        if (!value) return true; // Let required handle empty values
        return value === value.trim();
    }, 'Field may not contain leading or trailing spaces.');

    $('#LoginForm').validate({
        ignore: ["input[type=hidden]"], /* ignore hidden fields */
        // debug: true, /* prevent form submission for debugging */
        rules: {
            Login: { 
                required: true,               
                usernameOrEmail: true,
                noEdgeSpace: true
            },
            Password: { 
                required: true, 
                minlength: 8,
                maxlength: 128,
                strongPassword: true,
                noEdgeSpace: true
            }
        },
        messages: {
            Login: {
                required: "Please enter username or email.",               
                usernameOrEmail: "Enter a valid username (3-50 characters) or email (max 254 characters).",
                noEdgeSpace: "Username may not contain leading or trailing spaces."
            },
            Password: {
                required: "Please enter password.",
                minlength: "Password must be at least 8 characters.",
                maxlength: "Password must not exceed 128 characters.",
                noEdgeSpace: "Password may not contain leading or trailing spaces.",
                strongPassword: "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character."                
            }
        },
        submitHandler: function(form) {
            // Only submit if validation passes
            form.submit();
        }
    });
}

