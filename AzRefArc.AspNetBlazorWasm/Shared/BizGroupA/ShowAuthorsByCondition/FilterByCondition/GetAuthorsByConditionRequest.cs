using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzRefArc.AspNetBlazorWasm.Shared.BizGroupA.ShowAuthorsByCondition.FilterByCondition
{
    // Web API の入力パラメータチェック用クラス
    // (ViewModel クラスと違い、エラーメッセージは不要なため設定していない)
    [CustomValidation(typeof(GetAuthorsByConditionRequest), "CheckRequireState")]
    [CustomValidation(typeof(GetAuthorsByConditionRequest), "CheckRequirePhone")]
    [CustomValidation(typeof(GetAuthorsByConditionRequest), "CheckRequireContract")]
    [CustomValidation(typeof(GetAuthorsByConditionRequest), "CheckRequireAuFname")]
    [CustomValidation(typeof(GetAuthorsByConditionRequest), "CheckAtLeastOneCheck")]
    public class GetAuthorsByConditionRequest
    {
        public bool IsEnabledState { get; set; }
        public bool IsEnabledPhone { get; set; }
        public bool IsEnabledContract { get; set; }
        public bool IsEnabledAuFname { get; set; }

        [RegularExpression(@"^[A-Z]{2}$")]
        public string State { get; set; } = String.Empty;

        [RegularExpression(@"^\d{3} \d{3}-\d{4}$")]
        public string Phone { get; set; } = String.Empty;

        public bool Contract { get; set; }

        [RegularExpression(@"^[\u0020-\u007e]{1,20}$")]
        public string AuFname { get; set; } = String.Empty;

        public static ValidationResult CheckRequireState(GetAuthorsByConditionRequest vm, ValidationContext ctx)
        {
            if (vm.IsEnabledState == true && string.IsNullOrEmpty(vm.State) == true)
                return new ValidationResult("CheckRequireState", new List<string>() { "IsEnabledState", "State" });
            return ValidationResult.Success!;
        }

        public static ValidationResult CheckRequirePhone(GetAuthorsByConditionRequest vm, ValidationContext ctx)
        {
            if (vm.IsEnabledPhone == true && string.IsNullOrEmpty(vm.Phone) == true)
                return new ValidationResult("CheckRequirePhone", new List<string>() { "IsEnabledPhone", "Phone" });
            return ValidationResult.Success!;
        }

        public static ValidationResult CheckRequireContract(GetAuthorsByConditionRequest vm, ValidationContext ctx)
        {
            if (vm.IsEnabledContract == true)
                return new ValidationResult("CheckRequireContract", new List<string>() { "IsEnabledContract", "Contract" });
            return ValidationResult.Success!;
        }

        public static ValidationResult CheckRequireAuFname(GetAuthorsByConditionRequest vm, ValidationContext ctx)
        {
            if (vm.IsEnabledAuFname == true && string.IsNullOrEmpty(vm.AuFname) == true)
                return new ValidationResult("CheckRequireAuFname", new List<string>() { "IsEnabledAuFname", "AuFname" });
            return ValidationResult.Success!;
        }

        public static ValidationResult CheckAtLeastOneCheck(GetAuthorsByConditionRequest vm, ValidationContext ctx)
        {
            if (vm.IsEnabledState == false && vm.IsEnabledPhone == false && vm.IsEnabledContract == false && vm.IsEnabledAuFname == false)
                return new ValidationResult("CheckAtLeastOneCheck", new List<string>() { "IsEnabledState", "IsEnabledPhone", "IsEnabledContract", "IsEnabledAuFname" });
            return ValidationResult.Success!;
        }
    }
}
