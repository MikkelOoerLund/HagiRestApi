using AutoMapper;
using FluentValidation;
using HagiDatabaseDomain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace HagiRestApi.Controllers
{


    public class GetAllUsersRequest : IRequest<List<User>>
    {
    }


    public class GetAllUsersRequestHandler : IRequestHandler<GetAllUsersRequest, List<User>>
    {
        private readonly UserRepository _userRepository;

        public GetAllUsersRequestHandler(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> Handle(GetAllUsersRequest request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync();
            return users;
        }
    }

    public class GetUserWithIdRequest : IRequest<User>
    {
        public int UserId { get; set; }
    }


    public class GetUserWithIdRequestValidator : AbstractValidator<GetUserWithIdRequest>
    {
        public GetUserWithIdRequestValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .NotNull();
        }
    }

    public class GetUserWithIdRequestHandler : IRequestHandler<GetUserWithIdRequest, User>
    {
        private readonly UserRepository _userRepository;

        public GetUserWithIdRequestHandler(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Handle(GetUserWithIdRequest request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;

            var user = await _userRepository.GetAsync(userId);
            return user;
        }
    }


    public class GetUserWithNameRequest : IRequest<User>
    {
        public string UserName { get; set; }
    }


    public class GetUserWithNameRequestHandler : IRequestHandler<GetUserWithNameRequest, User>
    {
        private readonly UserRepository _userRepository;

        public GetUserWithNameRequestHandler(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Handle(GetUserWithNameRequest request, CancellationToken cancellationToken)
        {
            var userName = request.UserName;
            var user = await _userRepository.GetUserWithNameAsync(userName);
            return user;
        }
    }


    public class CreateUserRequest : IRequest<User>
    {
        public UserAuthenticationDTO UserAuthenticationDTO { get; set; }
    }


    public class CreateUserRequestHandler : IRequestHandler<CreateUserRequest, User>
    {
        private readonly IMapper _mapper;
        private readonly UserRepository _userRepository;

        public CreateUserRequestHandler(IMapper mapper, UserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<User> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            var userAuthentication = request.UserAuthenticationDTO;
            var user = _mapper.Map<User>(userAuthentication);

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
            return user;
        }
    }


    public class UpdateUserRequest : IRequest<User>
    {
        public int UserId { get; set; }
        public UserAuthenticationDTO UserAuthenticationDTO { get; set; }
    }

    public class UpdateUserRequestHandler : IRequestHandler<UpdateUserRequest, User>
    {
        private readonly IMapper _mapper;
        private readonly UserRepository _userRepository;

        public UpdateUserRequestHandler(IMapper mapper, UserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }


        public async Task<User> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var userAuthentication = request.UserAuthenticationDTO;

            var userValueContainer = _mapper.Map<User>(userAuthentication);
            var userToUpdate = await _userRepository.GetAsync(userId);

            _userRepository.SetValues(userToUpdate, userValueContainer);
            await _userRepository.SaveChangesAsync();
            return userToUpdate;
        }
    }

    public class DeleteUserWithIdRequest : IRequest
    {
        public int UserId { get; set; }
    }


    public class DeleteUserWithIdRequestHandler : IRequestHandler<DeleteUserWithIdRequest>
    {
        private readonly UserRepository _userRepository;

        public DeleteUserWithIdRequestHandler(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(DeleteUserWithIdRequest request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var userToDelete = await _userRepository.GetAsync(userId);

            _userRepository.Remove(userToDelete);
            await _userRepository.SaveChangesAsync();
        }
    }


    public class CustomRegexConstraint : IRouteConstraint
    {
        private readonly string _pattern;

        public CustomRegexConstraint(string pattern)
        {
            _pattern = pattern;
        }

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (values.TryGetValue(routeKey, out var value) && value != null)
            {
                var stringValue = value.ToString();
                return Regex.IsMatch(stringValue, _pattern);
            }

            return false;
        }
    }


    //public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    //    where TRequest : IRequest<TResponse>
    //{
    //    private readonly IEnumerable<IValidator<TRequest>> _validators;

    //    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    //    {
    //        _validators = validators;
    //    }

    //    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    //    {
    //        //var validationContext = new ValidationContext(request);
    //        //var failures = _validators
    //        //    .Select(x => x.Validate(validationContext))
    //        //    .SelectMany(x => x.Errors)
    //        //    .Where(x => x != null)
    //        //    .ToList();

    //        //var hasAnyValidationFailed = failures.Any();

    //        //if (hasAnyValidationFailed)
    //        //{
    //        //    throw new valid
    //        //}

    //        throw new NotImplementedException();
    //    }
    //}


    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private IMediator _mediator;


        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var request = new GetAllUsersRequest();
            var users = await _mediator.Send(request);
            return Ok(users);
        }

        [HttpGet("{id:int}", Name = "GetUserWithId")]
        public async Task<IActionResult> GetUserWithId(int id, [FromServices] IValidator<GetUserWithIdRequest> requestValidator)
        {

            var request = new GetUserWithIdRequest()
            {
                UserId = id,
            };

            var validationResult = requestValidator.Validate(request);

            if (validationResult.IsValid == false)
            {
                var modelStateDictionary = new ModelStateDictionary();

                foreach (var error in validationResult.Errors)
                {
                    var propertyName = error.PropertyName;
                    var errorMessage = error.ErrorMessage;

                    modelStateDictionary.AddModelError(propertyName, errorMessage);
                }

                return ValidationProblem(modelStateDictionary);
            }


            var user = await _mediator.Send(request);
            return Ok(user);
        }

        [HttpGet("{name:alpha}", Name = "GetUserWithName")]
        public async Task<IActionResult> GetUserWithName(string name)
        {
            var request = new GetUserWithNameRequest()
            {
                UserName = name,
            };

            var user = await _mediator.Send(request);
            if (user == null) return BadRequest($"No user has the given user name: {name}");
            return Ok(user);
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserAuthenticationDTO userAuthenticationDTO)
        {
            var request = new CreateUserRequest()
            {
                UserAuthenticationDTO = userAuthenticationDTO,
            };

            var user = await _mediator.Send(request);
            var routeValues = new { id = user.UserId };
            return CreatedAtRoute("GetUserWithId", routeValues, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserAuthenticationDTO userAuthenticationDTO)
        {
            var request = new UpdateUserRequest()
            {
                UserId = id,
                UserAuthenticationDTO = userAuthenticationDTO,
            };

            var user = await _mediator.Send(request);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var request = new DeleteUserWithIdRequest()
            {
                UserId = id,
            };

            await _mediator.Send(request);

            return NoContent();
        }
    }
}
