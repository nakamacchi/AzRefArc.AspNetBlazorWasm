﻿using System.ComponentModel.DataAnnotations;

namespace AzRefArc.AspNetBlazorWasm.Components.Pages.BizGroupD.ShowAuthorsByTopN
{
    public class FilterByTopNViewModel
    {
        [Required(ErrorMessage = "件数を入力してください。")]
        [RegularExpression("[1-9]{1}[0-9]{0,}", ErrorMessage = "1以上の整数値を入力してください。")]
        public string Count { get; set; } = "";
    }
}
