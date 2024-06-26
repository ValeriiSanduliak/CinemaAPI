﻿using CinemaAPI.Data;
using CinemaAPI.DTOs;
using CinemaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly CinemaDbContext appDbContext;

        public MediaController(CinemaDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult<List<Media>>> onGetAsync()
        {
            var media = await appDbContext
                .Media.Select(item => new
                {
                    item.MediaId,
                    item.MovieDescription,
                    item.MoviePhoto,
                    item.MovieTrailer
                })
                .ToListAsync();
            return Ok(media);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Media>> onGetMediaAsync(int id)
        {
            var media = await appDbContext.Media.FindAsync(id);
            if (media == null)
            {
                return NotFound();
            }

            var result = new
            {
                media.MediaId,
                media.MovieDescription,
                media.MoviePhoto,
                media.MovieTrailer
            };

            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<Media>> onPostAsync([FromBody] MediaDTO mediaDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var media = new Media
                    {
                        MovieDescription = mediaDto.MovieDescription,
                        MoviePhoto = mediaDto.MoviePhoto,
                        MovieTrailer = mediaDto.MovieTrailer
                    };

                    appDbContext.Media.Add(media);
                    await appDbContext.SaveChangesAsync();
                    var createdMedia = await appDbContext.Media.FindAsync(media.MediaId);

                    return StatusCode(201, createdMedia);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPatch("{id}")]
        public async Task<ActionResult<Media>> OnPatchAsync(int id, [FromBody] MediaPatchDTO media)
        {
            try
            {
                var existingMedia = await appDbContext.Media.FindAsync(id);
                if (existingMedia == null)
                {
                    return NotFound();
                }

                if (media.MovieDescription != null)
                {
                    existingMedia.MovieDescription = media.MovieDescription;
                }

                if (media.MoviePhoto != null)
                {
                    existingMedia.MoviePhoto = media.MoviePhoto;
                }

                if (media.MovieTrailer != null)
                {
                    existingMedia.MovieTrailer = media.MovieTrailer;
                }

                await appDbContext.SaveChangesAsync();
                return Ok(existingMedia);
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency conflict
                return Conflict("The media has been modified or deleted by another process.");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Media>> onDeleteAsync(int id)
        {
            var media = await appDbContext.Media.FindAsync(id);
            if (media == null)
            {
                return NotFound();
            }
            appDbContext.Media.Remove(media);
            await appDbContext.SaveChangesAsync();
            return Ok(media);
        }
    }
}
