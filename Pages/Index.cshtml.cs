using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VDIProject.Data;
using VDIProject.Models;

namespace VDIProject.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDBContext _context;

        public IndexModel(AppDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public long SelectedValue { get; set; }
        [BindProperty]
        public long Reward { get; set; }
        [BindProperty]
        public long Total { get; set; }
        public string CalculationResult { get; set; }
        public string TransactionID { get; set; }
        public List<SelectListItem> DropdownItems { get; set; }
        public async Task OnGetAsync()
        {
            DropdownItems = await _context.CustomerTypes
            .Select(item => new SelectListItem
            {
                Value = item.TypeID.ToString(),
                Text = item.Name
            }).ToListAsync();
        }
        public async Task<IActionResult> OnPostTransaction()
        {
            if (Total > 0 && SelectedValue > 0)
            {
                var type = await _context.CustomerTypes.FindAsync(SelectedValue);

                if (type != null)
                {
                    Decimal discount = 0;
                    if (Reward >= 100 && Reward <= 300)
                    {
                        discount = Total * Convert.ToDecimal(type.Div) / 100 + type.Low;
                    }
                    else if (Reward > 300 && Reward <= 500)
                    {
                        discount = Total * Convert.ToDecimal(type.Div) / 100 + type.Mid;
                    }
                    else if (Reward > 500)
                    {
                        discount = Total * Convert.ToDecimal(type.Div) / 100 + type.High;
                    }

                    TransactionID = await GenerateTransactionIdAsync();

                    var transaction = new TotalTransaction
                    {
                        TypeID = type.TypeID,
                        SubTotal = Total,
                        Discount = discount,
                        GrandTotal = Total - discount,
                        RewardPoints = Reward,
                        TransactionID = TransactionID
                    };

                    _context.Totals.Add(transaction);
                    await _context.SaveChangesAsync();

                    CalculationResult = Convert.ToString(Total - discount);
                }
                else
                {
                    CalculationResult = "Selected type not found.";
                }
            }
            else
            {
                CalculationResult = "Wrong Input";
            }

            await LoadDropdownItemsAndTransactionsAsync();
            return Page();
        }
        private async Task<string> GenerateTransactionIdAsync()
        {
            string datePart = DateTime.UtcNow.ToString("yyyyMMdd");

            var lastTransaction = await _context.Totals
                .Where(t => t.TransactionID.StartsWith(datePart))
                .OrderByDescending(t => t.TransactionID)
                .FirstOrDefaultAsync();

            long nextNumber = 1;
            if (lastTransaction != null)
            {
                nextNumber = lastTransaction.ID + 1;
            }

            string formattedNumber = nextNumber.ToString("D5");

            return $"{datePart}_{formattedNumber}";
        }
        private async Task LoadDropdownItemsAndTransactionsAsync()
        {
            DropdownItems = await _context.CustomerTypes
                .Select(item => new SelectListItem
                {
                    Value = item.TypeID.ToString(),
                    Text = item.Name
                }).ToListAsync();
        }

        [BindProperty]
        public string InputString { get; set; }
        public string ProcessedString { get; set; }
        public async void OnPostReversal()
        {
            if (!string.IsNullOrEmpty(InputString))
            {
                string first = InputString.Substring(0, InputString.Length / 2);
                string second = InputString.Substring((InputString.Length / 2));
                first = Reversal(first);
                second = Reversal(second);
                ProcessedString = first + second;
            }
            await LoadDropdownItemsAndTransactionsAsync();
        }
        private string Reversal(string input)
        {
            int l = 0;
            int r = input.Length - 1;
            char[] c = input.ToCharArray();
            while (l < r)
            {
                char temp = c[l];
                c[l] = c[r];
                c[r] = temp;
                l++;
                r--;
            }
            return new string(c);
        }

        [BindProperty]
        public string FirstString { get; set; }
        [BindProperty]
        public string SecondString { get; set; }
        public string IsItValid { get; set; }
        public async void OnPostValidAnagram()
        {
            if (!string.IsNullOrEmpty(FirstString) && !string.IsNullOrEmpty(SecondString))
            {
                string[] FirstArray = FirstString.Replace("\"", "").Split(new[] { ", " }, StringSplitOptions.None);
                string[] SecondArray = SecondString.Replace("\"", "").Split(new[] { ", " }, StringSplitOptions.None);

                if (FirstArray.Length != SecondArray.Length)
                {
                    IsItValid = "Wrong Input";
                    await LoadDropdownItemsAndTransactionsAsync();
                    return;
                }

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < FirstArray.Length; i++)
                {
                    if (IsValidAnagram(FirstArray[i], SecondArray[i]))
                    {
                        sb.Append("1");
                    }
                    else
                    {
                        sb.Append("0");
                    }
                }
                IsItValid = sb.ToString();
                await LoadDropdownItemsAndTransactionsAsync();
                return;
            }
            IsItValid = "Wrong Input";
            await LoadDropdownItemsAndTransactionsAsync();
        }
        private bool IsValidAnagram(string first, string second)
        {
            Dictionary<char, int> keyValuePairs = new Dictionary<char, int>();
            foreach (char c in first)
            {
                if (keyValuePairs.ContainsKey(c))
                {
                    keyValuePairs[c]++;
                }
                else
                {
                    keyValuePairs.Add(c, 1);
                }
            }

            foreach (char c in second)
            {
                if (keyValuePairs.ContainsKey(c))
                {
                    if (keyValuePairs[c] == 0)
                    {
                        return false;
                    }
                    keyValuePairs[c]--;
                }
                else
                {
                    return false;
                }
            }

            foreach (var keyValue in keyValuePairs.Values)
            {
                if (keyValue != 0)
                {
                    return false;
                }
            }

            return true;
        }


    }
}
