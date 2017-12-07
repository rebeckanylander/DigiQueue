using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigiQueue.Models.Hubs
{
    public class DigiHub : Hub
    {
        static List<Problem> waitingList = new List<Problem>();

        //Skicka meddelande i infobox - digimaster
        public Task InfoSend(string message) 
        {
            return Clients.All.InvokeAsync("onInfoSend", message);
        }

        //Ta bort student från waiting list - digimaster
        public Task DeleteWaitingListItem(string alias) 
        {
            Problem problem = waitingList.SingleOrDefault(p => p.Alias == alias);
            if (problem != null)
            {
                waitingList.Remove(problem);
            }
            string json = JsonConvert.SerializeObject(waitingList);
            return Clients.All.InvokeAsync("onUpdateWaitingListItem", json);
        }

        //Skicka meddelande i chatten - digistudent
        public Task ChatSend(string message) 
        {
            return Clients.All.InvokeAsync("onChatSend", message);
        }

        //Lägga till sig på listan - digistudent
        public Task AddWaitingListItem(string jsonString) 
        {
            var jsonObj = JsonConvert.DeserializeObject<Problem>(jsonString);
            if ((waitingList.SingleOrDefault(p => p.Alias == jsonObj.Alias)) == null)
            {
                waitingList.Add(jsonObj);
            }

            string json = JsonConvert.SerializeObject(waitingList);
            return Clients.All.InvokeAsync("onUpdateWaitingListItem", json);
        }

        //Ta bort sig själv på listan - digistudent
        public Task RemoveSelfFromWaitingList(string alias) 
        {
            
            Problem problem = waitingList.SingleOrDefault(p => p.Alias == alias);

            if (problem != null)
            {
                waitingList.Remove(problem);
            }
            string json = JsonConvert.SerializeObject(waitingList);
            return Clients.All.InvokeAsync("onUpdateWaitingListItem", json);
       
        }
    }
}
