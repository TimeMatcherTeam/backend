using Microsoft.AspNetCore.Identity;

namespace TimeMatcher.Api.Errors;

public class RussianIdentityErrorDescriber: IdentityErrorDescriber
{
    public override IdentityError PasswordMismatch()
    {
        return new IdentityError
        {
            Code = nameof(PasswordMismatch), 
            Description = "Неверный текущий пароль"
        };
    }

    public override IdentityError PasswordRequiresDigit()
    {
        return new IdentityError
        {
            Code = nameof(PasswordRequiresDigit),
            Description = "Пароль должен содержать хотя бы 1 цифру"
        };
    }

    public override IdentityError PasswordRequiresLower()
    {
        return new IdentityError()
        {
            Code = nameof(PasswordRequiresLower),
            Description = "Пароль должен содержать хотя бы одну строчную букву (a-z)"
        };
    }

    public override IdentityError PasswordRequiresUpper()
    {
        return new IdentityError
        {
            Code = nameof(PasswordRequiresUpper),
            Description = "Пароль должен содержать хотя бы одну заглавную букву (A-Z)"
        };
    }

    public override IdentityError PasswordTooShort(int length)
    {
        return new IdentityError 
        { 
            Code = nameof(PasswordTooShort), 
            Description = $"Пароль слишком короткий. Минимальная длина: {length} символов." 
        };
    }

    public override IdentityError PasswordRequiresNonAlphanumeric()
    {
        return new IdentityError 
        { 
            Code = nameof(PasswordRequiresNonAlphanumeric), 
            Description = "Пароль должен содержать хотя бы один спецсимвол (например: @, #, $, %)." 
        };
    }
    
    public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
    {
        return new IdentityError 
        { 
            Code = nameof(PasswordRequiresUniqueChars), 
            Description = $"Пароль должен содержать как минимум {uniqueChars} уникальных (неповторяющихся) символов." 
        };
    }
    
    public override IdentityError DuplicateEmail(string email)
    {
        return new IdentityError 
        { 
            Code = nameof(DuplicateEmail), 
            Description = $"Почта '{email}' уже используется другим пользователем." 
        };
    }

    public override IdentityError DuplicateUserName(string userName)
    {
        return new IdentityError 
        { 
            Code = nameof(DuplicateUserName), 
            Description = $"Пользователь с логином '{userName}' уже зарегистрирован." 
        };
    }

    public override IdentityError InvalidEmail(string email)
    {
        return new IdentityError 
        { 
            Code = nameof(InvalidEmail), 
            Description = $"Введен некорректный адрес электронной почты." 
        };
    }

    public override IdentityError InvalidUserName(string userName)
    {
        return new IdentityError 
        { 
            Code = nameof(InvalidUserName), 
            Description = "Логин содержит недопустимые символы." 
        };
    }


}