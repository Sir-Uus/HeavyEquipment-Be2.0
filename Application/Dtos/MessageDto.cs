using System;

namespace Application.Dtos;

public class MessageDto
{
  public string Content { get; set; } = default!;
  public string Username { get; set; } = default!;
  public DateTime SentAt { get; set; }
}
