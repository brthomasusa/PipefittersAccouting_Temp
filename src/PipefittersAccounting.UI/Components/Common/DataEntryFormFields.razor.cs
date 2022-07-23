#pragma warning disable CS8602

using Microsoft.AspNetCore.Components;
using Blazorise;
using Blazorise.Snackbar;
using FluentValidation;
using PipefittersAccounting.SharedModel;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.UI.Components.Common;
using PipefittersAccounting.UI.Finance.Validators;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Components.Common
{
    public partial class DataEntryFormFields<TWriteModel>
    {
        [Parameter] public Validations? Validations { get; set; }
        [Parameter] public RenderFragment? DataEntryFields { get; set; }
        [Parameter] public TWriteModel? WriteModel { get; set; }

    }
}