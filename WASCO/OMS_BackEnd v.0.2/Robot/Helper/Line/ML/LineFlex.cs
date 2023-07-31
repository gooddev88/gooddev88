using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Helper.Line.ML {
    public class LineFlex {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Root {
            public string type { get; set; }
            public Hero hero { get; set; }
            public Body body { get; set; }
            public Footer footer { get; set; }
        }


        public class Action {
            public string type { get; set; }
            public string uri { get; set; }
            public string label { get; set; }
        }

        public class Hero {
            public string type { get; set; }
            public string url { get; set; }
            public string size { get; set; }
            public string aspectRatio { get; set; }
            public string aspectMode { get; set; }
            public Action action { get; set; }
        }

        public class Content {
            public string type { get; set; }
            public string text { get; set; }
            public string color { get; set; }
            public string size { get; set; }
            public int flex { get; set; }
            public bool? wrap { get; set; }
            public string layout { get; set; }
            public string spacing { get; set; }
     
            public string weight { get; set; }
            public string margin { get; set; }
            public string style { get; set; }
            public string height { get; set; }
            public Action action { get; set; }
        }

        public class Body {
            public string type { get; set; }
            public string layout { get; set; }
            public List<Content> contents { get; set; }
        }

        public class Footer {
            public string type { get; set; }
            public string layout { get; set; }
            public string spacing { get; set; }
            public List<Content> contents { get; set; }
            public int flex { get; set; }
        }

      


    }
    /**
    {
      "type":"bubble",
      "hero":{
         "type":"image",
         "url":"https://ky.kypos.cc/sale/assets/img/bazaar.jpg",
         "size":"full",
         "aspectRatio":"20:13",
         "aspectMode":"cover",
         "action":{
            "type":"uri",
            "uri":"http://linecorp.com/"
         }
      },
      "body":{
         "type":"box",
         "layout":"vertical",
         "contents":[
            {
               "type":"text",
               "text":"ยอดเบิกเงินซื้อของ",
               "weight":"bold",
               "size":"xl"
            },
            {
               "type":"box",
               "layout":"vertical",
               "margin":"lg",
               "spacing":"sm",
               "contents":[
                  {
                     "type":"box",
                     "layout":"baseline",
                     "spacing":"sm",
                     "contents":[
                        {
                           "type":"text",
                           "text":"ยอดรวม",
                           "color":"#aaaaaa",
                           "size":"sm",
                           "flex":1
                        },
                        {
                           "type":"text",
                           "text":"12,000 ฿",
                           "wrap":true,
                           "color":"#666666",
                           "size":"xxl",
                           "flex":5
                        }
                     ]
                  },
                  {
                     "type":"box",
                     "layout":"baseline",
                     "spacing":"sm",
                     "contents":[
                        {
                           "type":"text",
                           "text":"วันที่",
                           "color":"#aaaaaa",
                           "size":"sm",
                           "flex":1
                        },
                        {
                           "type":"text",
                           "text":"2/3/2022",
                           "wrap":true,
                           "color":"#666666",
                           "size":"sm",
                           "flex":5
                        }
                     ]
                  }
               ]
            }
         ]
      },
      "footer":{
         "type":"box",
         "layout":"vertical",
         "spacing":"sm",
         "contents":[
            {
               "type":"button",
               "style":"link",
               "height":"sm",
               "action":{
                  "type":"uri",
                  "label":"รายละเอียด",
                  "uri":"https://linecorp.com"
               }
            },
            {
               "type":"spacer",
               "size":"sm"
            }
         ],
         "flex":0
      }
}
       **/

}
