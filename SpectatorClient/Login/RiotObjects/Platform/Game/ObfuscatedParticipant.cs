﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectatorClient.Login.RiotObjects.Platform.Game
{
    public class ObfuscatedParticipant : Participant
    {
        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        private string type = "com.riotgames.platform.game.ObfruscatedParticipant";

        public ObfuscatedParticipant()
        {
        }

        public ObfuscatedParticipant(Callback callback)
        {
            this.callback = callback;
        }

        public ObfuscatedParticipant(TypedObject result)
        {
            base.SetFields(this, result);
        }

        public delegate void Callback(ObfuscatedParticipant result);

        private Callback callback;

        public override void DoCallback(TypedObject result)
        {
            base.SetFields(this, result);
            callback(this);
        }

        [InternalName("badges")]
        public Int32 Badges { get; set; }

        [InternalName("index")]
        public Int32 Index { get; set; }

        [InternalName("clientInSynch")]
        public Boolean ClientInSynch { get; set; }

        [InternalName("gameUniqueId")]
        public Int32 GameUniqueId { get; set; }

        [InternalName("pickMode")]
        public Int32 PickMode { get; set; }
    }
}
