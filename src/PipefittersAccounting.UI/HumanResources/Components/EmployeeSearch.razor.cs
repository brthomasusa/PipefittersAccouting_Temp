#pragma warning disable CS8622

using Microsoft.AspNetCore.Components;

namespace PipefittersAccounting.UI.HumanResources.Components
{
    public partial class EmployeeSearch
    {
        private Timer? _timer;

        public string? SearchTerm { get; set; }

        [CascadingParameter(Name = "SearchChangedEventHandler")]
        public EventCallback<string> OnSearchChanged { get; set; }

        private void SearchChanged()
        {
            if (_timer != null)
                _timer.Dispose();

            _timer = new Timer(OnTimerElapsed, null, 500, 0);
        }

        private void OnTimerElapsed(object sender)
        {
            OnSearchChanged.InvokeAsync(SearchTerm);
            _timer!.Dispose();
        }
    }
}