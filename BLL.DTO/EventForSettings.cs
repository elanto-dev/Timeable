using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
{
    public class EventForSettings
    {
        public int Id { get; set; }

        [Display(Name = nameof(StartDateTime), Prompt = nameof(StartDateTime), ResourceType = typeof(Resources.Domain.EventView.Events))]
        public string StartDateTime { get; set; } = default!;

        [Display(Name = nameof(EndDateTime), Prompt = nameof(EndDateTime), ResourceType = typeof(Resources.Domain.EventView.Events))]
        public string EndDateTime { get; set; } = default!;


        [Display(Name = nameof(Name), Prompt = nameof(Name), ResourceType = typeof(Resources.Domain.EventView.Events))]
        public string Name { get; set; } = default!;


        [Display(Name = nameof(Place), Prompt = nameof(Place), ResourceType = typeof(Resources.Domain.EventView.Events))]
        public string Place { get; set; } = default!;


        [Display(Name = nameof(ShowStartDateTime), Prompt = nameof(ShowStartDateTime), ResourceType = typeof(Resources.Domain.EventView.Events))]
        public string ShowStartDateTime { get; set; } = default!;


        [Display(Name = nameof(ShowEndDateTime), Prompt = nameof(ShowEndDateTime), ResourceType = typeof(Resources.Domain.EventView.Events))]
        public string ShowEndDateTime { get; set; } = default!;
    }
}
