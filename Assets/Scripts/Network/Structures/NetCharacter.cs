using UnityEngine;

namespace Assets.Scripts.Network.Structures
{
    [System.Serializable]
    public class NetCharacter
    {
        public string netId;
        public int _id;
        public string _name;
        public string _class;
        public string _race;
        public string _gender;
        public int _level;
        public Vector3 _pos;
        public float _rot;

        public NetCharacter(int _id, string _name, string _class, string _race, string _gender, int _level, Vector3 _pos, float _rot)
        {
            this._id = _id;   
            this._name = _name;   
            this._class = _class;   
            this._race = _race;   
            this._gender = _gender;   
            this._level = _level;   
            this._pos = _pos;   
            this._rot = _rot;
        }

        public void Update()
        {
     
        }

        internal void Clear()
        {
            this._id = 0;
            this._name = null;
            this._class = null;
            this._race = null;
            this._gender = null;
            this._level = 0;
            this._pos = Vector3.zero;
            this._rot = 0;
        }
    }
}
