using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser;

/// <summary>
/// Handler for processing UpdateUserCommand requests
/// </summary>
public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UpdateUserResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;

    /// <summary>
    /// Initializes a new instance of UpdateUserHandler
    /// </summary>
    /// <param name="userRepository">The user repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="passwordHasher"></param>
    public UpdateUserHandler(
        IUserRepository userRepository,
        IMapper mapper,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
    }

    /// <summary>
    /// Handles the UpdateUserCommand request
    /// </summary>
    /// <param name="request">The UpdateUser command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The user details if found</returns>
    public async Task<UpdateUserResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await new UpdateUserValidator().ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var byEmail = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (byEmail != null && byEmail.Id != request.Id)
            throw new InvalidOperationException($"User with email {request.Email} already exists");
        
        byEmail = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);
        if (byEmail != null && byEmail.Id != request.Id)
            throw new InvalidOperationException($"User with username {request.Username} already exists");

        var existingUser = await _userRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new KeyNotFoundException("User does not exist");

        _mapper.Map(request, existingUser);

        existingUser.Password = _passwordHasher.HashPassword(request.Password);

        await _userRepository.UpdateAsync(existingUser, cancellationToken);

        return _mapper.Map<UpdateUserResult>(existingUser);
    }
}
