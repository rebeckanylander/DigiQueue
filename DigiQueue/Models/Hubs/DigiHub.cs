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
        List<Problem> waitingList = new List<Problem>();

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
            return Clients.All.InvokeAsync("onDeleteWaitingListItem", json);
        }

        //Skicka meddelande i chatten - digistudent
        public Task ChatSend(string message) 
        {
            return Clients.All.InvokeAsync("onSend", message);
        }

        //Lägga till sig på listan - digistudent
        public Task AddWaitingListItem() 
        {
            throw new NotImplementedException();
        }

        //Ta bort sig själv på listan - digistudent
        public Task RemoveSelfFromWaitingList() 
        {
            throw new NotImplementedException();
        }
    }
}
