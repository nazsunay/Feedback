﻿using Feedback.Data;
using Feedback.Dto;
using Feedback.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class CommentController : ControllerBase
{
    private readonly _context _context;

    public CommentController(_context context)
    {
        _context = context;
    }

    // GET: api/comment
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DtoAddComment>>> GetComments()
    {
        var comments = await _context.Comments
            .Include(c => c.User)
            .Include(c => c.Replies)
            .ThenInclude(r => r.User)
            .Select(c => new DtoAddComment
            {
                Id = c.Id,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                UserId = c.UserId,
                OpinionId = c.OpinionId,
                ParentCommentId = c.ParentCommentId,
                Replies = c.Replies.Select(r => new DtoAddComment
                {
                    Id = r.Id,
                    Content = r.Content,
                    CreatedAt = r.CreatedAt,
                    UserId = r.UserId,
                    OpinionId = r.OpinionId,
                    ParentCommentId = r.ParentCommentId
                }).ToList()
            })
            .ToListAsync();

        return comments;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DtoAddComment>> GetComment(int id)
    {
        var comment = await _context.Comments
            .Include(c => c.User)
            .Include(c => c.Replies)
            .ThenInclude(r => r.User)
            .Where(c => c.Id == id)
            .Select(c => new DtoAddComment
            {
                Id = c.Id,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                UserId = c.UserId,
                OpinionId = c.OpinionId,
                ParentCommentId = c.ParentCommentId,
                Replies = c.Replies.Select(r => new DtoAddComment
                {
                    Id = r.Id,
                    Content = r.Content,
                    CreatedAt = r.CreatedAt,
                    UserId = r.UserId,
                    OpinionId = r.OpinionId,
                    ParentCommentId = r.ParentCommentId
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (comment == null)
        {
            return NotFound();
        }

        return comment;
    }

    [HttpPost]
    public async Task<ActionResult<DtoAddComment>> PostComment(DtoAddComment commentDto)
    {
        // Geçerli bir OpinionId kontrolü
        var opinionExists = await _context.Opinions.AnyAsync(o => o.Id == commentDto.OpinionId);
        if (!opinionExists)
        {
            return BadRequest("Geçersiz görüş ID.");
        }

        var comment = new Comment
        {
            Content = commentDto.Content,
            CreatedAt = DateTime.UtcNow,
            UserId = commentDto.UserId,
            OpinionId = commentDto.OpinionId,
            ParentCommentId = commentDto.ParentCommentId // Alt yorum için ID
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        commentDto.Id = comment.Id; // Yeni oluşturulan yorumun ID'sini ata

        return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, commentDto);
    }

    // PUT: api/comment/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutComment(int id, DtoAddComment commentDto)
    {
        if (id != commentDto.Id)
        {
            return BadRequest();
        }

        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            return NotFound();
        }

        comment.Content = commentDto.Content;
        comment.UserId = commentDto.UserId;

        _context.Entry(comment).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CommentExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        var comment = await _context.Comments
            .Include(c => c.Replies)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (comment == null)
        {
            return NotFound();
        }

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CommentExists(int id)
    {
        return _context.Comments.Any(e => e.Id == id);
    }
}
