using System;
using Application.Dtos;
using Domain.Entities;

namespace Application.Vm;

public class MessageVm
{
  public int Id { get; set; }
  public string Content { get; set; } = default!;
  public int UserId { get; set; }
  public UserSummaryDto User { get; set; } = default!;
  public DateTime SentAt {get; set;}
}
