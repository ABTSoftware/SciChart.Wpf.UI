using System;
using System.Collections.Generic;

namespace SciChart.Wpf.UI.Reactive.Services
{
    public enum ExampleFeedbackType
    {
        Frown,
        Smile
    }

    public class ExampleUsage
    {
        private int _visitCount;
        private int _secondsSpent;
        private int _interactions;
        private bool _viewedSource;
        private bool _exported;
        private ExampleFeedbackType? _feedbackType;
        private string _email;
        private string _feedbackText;

        public string ExampleID { get; set; }

        /// <summary>
        /// Total visits to the example
        /// </summary>
        public int VisitCount { get { return _visitCount; } set { _visitCount = value; LastUpdated = DateTime.UtcNow; } }

        /// <summary>
        /// Total time spent looking at the example
        /// </summary>
        public int SecondsSpent { get { return _secondsSpent; } set { _secondsSpent = value; LastUpdated = DateTime.UtcNow; } }

        /// <summary>
        /// Number of interactions with functional parts the example 
        /// </summary>
        public int Interactions { get { return _interactions; } set { _interactions = value; LastUpdated = DateTime.UtcNow; } }

        /// <summary>
        /// True if the user ever viewed the source of the example
        /// </summary>
        public bool ViewedSource { get { return _viewedSource; } set { _viewedSource = value; LastUpdated = DateTime.UtcNow; } }

        /// <summary>
        /// True if the user ever exported the example
        /// </summary>
        public bool Exported { get { return _exported; } set { _exported = value; LastUpdated = DateTime.UtcNow; } }

        /// <summary>
        /// User rating
        /// </summary>
        public ExampleFeedbackType? FeedbackType { get { return _feedbackType; } set { _feedbackType = value; LastUpdated = DateTime.UtcNow; } }

        /// <summary>
        /// Users email, if they would like to be contacted regarding their feedback
        /// </summary>
        public string Email { get { return _email; } set { _email = value; LastUpdated = DateTime.UtcNow; } }

        /// <summary>
        /// User comments
        /// </summary>
        public string FeedbackText { get { return _feedbackText; } set { _feedbackText = value; LastUpdated = DateTime.UtcNow; } }


        public DateTime LastUpdated { get; set; }
    }

    public class UsageData
    {
        public string UserId { get; set; }
        /// <summary>
        /// Usage dictionary keyed on ExampleId
        /// </summary>
        public List<ExampleUsage> Usage { get; set; }

    }

    public class ExampleRating
    {
        public string ExampleID { get; set; }
        /// <summary>
        /// Score based on usage stats like vists and time spent
        /// </summary>
        public float Popularity { get; set; }

        /// <summary>
        /// Score based on user feedback
        /// </summary>
        public float Rating { get; set; }

        // Idea - list of user feedback where user has consented to its use
    }

}
