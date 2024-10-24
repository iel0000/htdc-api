using AutoMapper;
using htdc_api.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace htdc_api.Controllers;

public class BaseController : ControllerBase
{
    public readonly ApplicationDbContext _context;
    public readonly UserManager<IdentityUser> _userManager;
    private ApplicationDbContext context;
    public readonly IMapper _mapper;

    public BaseController(ApplicationDbContext context, IMapper mapper, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _mapper = mapper;
        _userManager = userManager;
    }

}