
using System.Text.Json;
public static class JsonElementProperty{

    public static JsonElement GetStrProperty(this JsonElement obj,string keyMap){
        
        foreach(string key in keyMap.ToLower().Split(":"))
        {
            obj =  obj.GetProperty(key); 
        }
        return obj;
           
    }
}