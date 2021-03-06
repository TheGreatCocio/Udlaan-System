﻿using System;
using System.Collections.Generic;
using UdlaanSystem.DataAccess;
using UdlaanSystem.Properties;

namespace UdlaanSystem.Managers
{
    class ItemController
    {
        public ItemController()
        {

        }
        private static ItemController instance;

        public static ItemController Instance
        {
            get
            {
                if (instance == null)
                { instance = new ItemController(); }
                return instance;
            }
        }

        public ItemObject CheckIfMifareIsItem(string itemMifare)
        {
            ItemObject item = DALItem.Instance.GetItemByMifare(itemMifare);

            return item;            
        }


        public List<string[]> GetItemTypes()
        {
            return DALItem.Instance.GetItemTypes();
        }

        public List<string[]> GetItemManufacturers(int typeID)
        {
            return DALItem.Instance.GetItemManufacturers(typeID);
        }

        public List<string[]> GetItemModels(int manufacturerID, int typeID)
        {
            return DALItem.Instance.GetItemModels(manufacturerID, typeID);
        }

        public int CalculateNextID (int model, List<int> listOfIds) 
        {
            List<int> retrievedIds = DALItem.Instance.RetrieveIdInformation(model);

            int newID = 0;

            while (true)
            {
                newID++;
                if (!retrievedIds.Contains(newID) && !listOfIds.Contains(newID))
                {
                    break;
                }
            }
            return newID;
        }

        public bool InsertItems(List<ItemObject> itemsToBeInsert)
        {
            return DALItem.Instance.InsertItemsIntoDB(itemsToBeInsert);
        }

        public string GetItemModelName(int modelID)
        {
            return DALItem.Instance.GetItemModelName(modelID);
        }

        public bool FindItemByID(ItemObject item)
        {
            return DALItem.Instance.FindItemByID(item);
        }

        public bool UpdateMifareOnItem(ItemObject item)
        {
            return DALItem.Instance.UpdateMifareOnItem(item);
        }
    }
}
