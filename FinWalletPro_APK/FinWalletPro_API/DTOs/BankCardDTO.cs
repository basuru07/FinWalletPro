namespace FinWalletPro_APK.FinWalletPro_API.DTOs
{
    public class BankCardDto
    {
        public Guid Id { get; set; }
        public string? CardHolderName { get; set; }
        public string? Last4Digits { get; set; }
        public string? CardType { get; set; }
        public string? ExpiryMonth { get; set; }
        public string? ExpiryYear { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
    }

    public class AddBankCardDto
    {
        public string? CardHolderName { get; set; }
        public string? CardNumber { get; set; }
        public string? ExpiryMonth { get; set; }
        public string? ExpiryYear { get; set; }
        public string? CVV { get; set; }
        public bool SetAsDefault { get; set; }
    }

    public class BeneficiaryDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? WalletNumber { get; set; }
        public string? Nickname { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class AddBeneficiaryDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? WalletNumber { get; set; }
        public string? Nickname { get; set; }
    }

    public class UpdateBeneficiaryDto
    {
        public string? Nickname { get; set; }
    }

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public List<string> Errors { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string message = "Operation successful")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse<T> ErrorResponse(string message, List<string> errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }

    }
}