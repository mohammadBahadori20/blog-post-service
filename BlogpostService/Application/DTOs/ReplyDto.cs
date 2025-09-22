using System.ComponentModel.DataAnnotations;
using BlogpostService.Domain;

namespace BlogpostService.Application.DTOs;

public class ReplyDto
{
    public long RepliesCount { get; set; }
    public List<CommentDto> Replies { get; set; } = new();
    public bool HasMore { get; set; }
    public int? NextPage { get; set; }
}