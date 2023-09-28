using System.ComponentModel.DataAnnotations;

namespace PhoneStore.Enums;

public enum SortState
{
    [Display(Name = "Высокий")]
    NameAsc,
    NameDesc,
    AgeAsc,
    AgeDesc,
    BrandAsc,
    BrandDesc
}