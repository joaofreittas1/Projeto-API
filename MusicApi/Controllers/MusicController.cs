using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MusicApi.Data;
using MusicApi.Data.Dtos;
using MusicApi.Models;

namespace MusicApi.Controllers;

[ApiController]
[Route("[controller]")]
public class MusicController : ControllerBase
{
    private MusicContext _context;
    private IMapper _mapper;

    public MusicController(MusicContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult AddMusica([FromBody] CreateMusicDto musicDto)
    {
        Music music = _mapper.Map<Music>(musicDto);

        _context.Musics.Add(music);
        _context.SaveChanges();

        return  CreatedAtAction(nameof(MusicId), new { id = music.Id }, music);
    }

    [HttpGet]
    public IEnumerable<Music> GetMusica([FromQuery] int skip = 0 , [FromQuery] int take = 10)
    {
        return _context.Musics.Skip(skip).Take(take);
    }

    [HttpGet("{id}")]
    public IActionResult MusicId(int id) 
    {
        var music =  _context.Musics.FirstOrDefault(music => music.Id == id);

        if (music == null) { return NotFound(); }

        return Ok(music);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateMusic(int id, [FromBody]  UpdateMusicDto musicDto) 
    {
        var music = _context.Musics.FirstOrDefault(music => music.Id == id);

        if (music == null) { return NotFound(); }

        _mapper.Map(musicDto,music);

        _context.SaveChanges();

        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult UpdateMusicPartial(int id, JsonPatchDocument<UpdateMusicDto> jsonPatch)
    {
        var music = _context.Musics.FirstOrDefault(music => music.Id == id);

        if (music == null) { return NotFound(); }

        var musicUpdate = _mapper.Map<UpdateMusicDto>(music);

        jsonPatch.ApplyTo(musicUpdate, ModelState);

        if (!TryValidateModel(musicUpdate)) { return ValidationProblem(ModelState); }

        _mapper.Map(musicUpdate, music);

        _context.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteMusic(int id)
    {
        var music = _context.Musics.FirstOrDefault(music => music.Id == id);

        if (music == null) { return NotFound(); }

        _context.Remove(music);

        _context.SaveChanges();

        return NoContent();
    }
}
