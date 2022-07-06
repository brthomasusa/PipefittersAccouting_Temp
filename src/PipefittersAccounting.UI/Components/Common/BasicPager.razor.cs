using Microsoft.AspNetCore.Components;

namespace PipefittersAccounting.UI.Components.Common
{
    public partial class BasicPager
    {
        private bool[] isActive = { false, true, false };

        private void Previous()
        {
            if (isActive[0])
                return;

            if (isActive[1])
            {
                SetActive("1");
                return;
            }

            if (isActive[2])
            {
                SetActive("2");
                return;
            }
        }

        private void Next()
        {
            if (isActive[0])
            {
                SetActive("2");
                return;
            }

            if (isActive[1])
            {
                SetActive("3");
                return;
            }

            if (isActive[2])
            {
                return;
            }
        }

        private void SetActive(string idx)
        {
            switch (idx)
            {
                case "1":
                    isActive[0] = true;
                    isActive[1] = false;
                    isActive[2] = false;
                    break;
                case "2":
                    isActive[0] = false;
                    isActive[1] = true;
                    isActive[2] = false;
                    break;
                case "3":
                    isActive[0] = false;
                    isActive[1] = false;
                    isActive[2] = true;
                    break;
                default:
                    break;
            }

        }
    }
}