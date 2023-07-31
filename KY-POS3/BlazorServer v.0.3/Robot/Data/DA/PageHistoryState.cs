using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Data.DA
{
    public class PageHistoryState
    {
        private List<string> previousPages;

        public PageHistoryState() {
            previousPages = new List<string>();
        }
        public void AddPageToHistory(string pageName) {
            previousPages.Add(pageName);
        }

        public string GetGoBackPage() {
            string urlreturn = "Menu/MainMenu";
            try {
                if (previousPages.Count > 1) {
                    // You add a page on initialization, so you need to return the 2nd from the last
                    urlreturn = previousPages.ElementAt(previousPages.Count - 2);
                    //previousPages.RemoveAt(previousPages.Count-1);
                    previousPages.RemoveRange(previousPages.Count - 2, 2);
                } else {
                    // Can't go back because you didn't navigate enough
                    if (previousPages.FirstOrDefault() != null) {
                        urlreturn = previousPages.FirstOrDefault();
                    }

                }
            } catch (Exception) { 
            }
        
            return urlreturn;
        }

        public bool CanGoBack() {
            return previousPages.Count > 1;
        }
    }
    }

