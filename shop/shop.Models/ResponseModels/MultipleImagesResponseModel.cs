using Newtonsoft.Json;
using shop.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace shop.Models.ResponseModels
{
    public class MultipleImagesResponseModel
    {
        public ImageResponseModel Image { get; set; }
                
        public ImageSaveStatus? Status { get; set; }

        public string Name { get; set; }
    }
}
