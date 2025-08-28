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

    /// <summary>
    /// Adiciona uma musica ao banco de dados
    /// </summary>
    /// <param name="musicDto"> Objeto com os campos necessários para criação de uma musica</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    [HttpPost]
    public IActionResult AddMusica([FromBody] CreateMusicDto musicDto)
    {
        Music music = _mapper.Map<Music>(musicDto);

        _context.Musics.Add(music);
        _context.SaveChanges();

        return CreatedAtAction(nameof(MusicId), new { id = music.Id }, music);
    }

    /// <summary>
    /// Retorna uma lista de musicas
    /// </summary>
    /// <param name="skip"> Número de musicas que serão puladas</param>
    /// <param name="take"> Número de musicas que serão retornadas</param>
    /// <returns>IEnumerable<Music></returns>
    [HttpGet]
    public IEnumerable<Music> GetMusica([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        return _context.Musics.Skip(skip).Take(take);
    }

    /// <summary>
    /// Retorna uma musica específica
    /// </summary>
    /// <param name="id"> ID da música a ser retornada</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso a música seja encontrada com sucesso</response>
    /// <response code="404">Caso a música não seja encontrada</response>
    [HttpGet("{id}")]
    public IActionResult MusicId(int id)
    {
        var music = _context.Musics.FirstOrDefault(music => music.Id == id);

        if (music == null) { return NotFound(); }

        return Ok(music);
    }

    /// <summary>
    /// Atualiza uma musica por completo
    /// </summary>
    /// <param name="id"> ID da música a ser atualizada</param>
    /// <param name="musicDto"> Objeto com os campos para atualização da música</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso a atualização seja feita com sucesso</response>
    /// <response code="404">Caso a música não seja encontrada</response>
    [HttpPut("{id}")]
    public IActionResult UpdateMusic(int id, [FromBody] UpdateMusicDto musicDto)
    {
        var music = _context.Musics.FirstOrDefault(music => music.Id == id);

        if (music == null) { return NotFound(); }

        _mapper.Map(musicDto, music);

        _context.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// Atualiza parcialmente uma musica
    /// </summary>
    /// <param name="id"> ID da música a ser atualizada</param>
    /// <param name="jsonPatch"> Objeto com os campos para atualização parcial da música</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso a atualização seja feita com sucesso</response>
    /// <response code="404">Caso a música não seja encontrada</response>
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

    /// <summary>
    /// Deleta uma musica
    /// </summary>
    /// <param name="id"> ID da música a ser deletada</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso a remoção seja feita com sucesso</response>
    /// <response code="404">Caso a música não seja encontrada</response>
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