using System;
using System.Collections.Generic;

namespace Unleashed_MVC.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public int? ReviewId { get; set; }

    public string? CommentContent { get; set; }

    public DateTimeOffset? CommentCreatedAt { get; set; }

    public DateTimeOffset? CommentUpdatedAt { get; set; }

    public int? ParentCommentId { get; set; }

    public virtual ICollection<Comment> InverseParentComment { get; set; } = new List<Comment>();

    public virtual Comment? ParentComment { get; set; }

    public virtual Review? Review { get; set; }

    public virtual ICollection<Comment> CommentParents { get; set; } = new List<Comment>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
