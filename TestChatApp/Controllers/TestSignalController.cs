﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestChatApp.Models;

namespace TestChatApp.Controllers
{
    public class TestSignalController : Controller
    {
        // GET: TestSignal
        public ActionResult Chat()
        {
            return View();
        }

        
    }
}