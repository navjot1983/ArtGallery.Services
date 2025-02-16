﻿// -----------------------------------------------------------------------
// Copyright (c) MumsWhoCode. All rights reserved.
// -----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using ArtGallery.Services.Api.Models.Artists;
using ArtGallery.Services.Api.Models.Artists.Exceptions;
using Moq;
using Xunit;

namespace ArtGallery.Services.Tests.Unit.Services.Foundations.Artists
{
    public partial class ArtistServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfArtistIsNullAndLogItAsync()
        {
            //given
            Artist nullArtist = null;
            var nullArtistException = new NullArtistException();

            var expectedArtistValidationException =
               new ArtistValidationException(nullArtistException);

            //when
            ValueTask<Artist> addArtistTask =
                this.artistService.AddArtistAsync(nullArtist);

            //then
            await Assert.ThrowsAsync<ArtistValidationException>(() =>
                addArtistTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedArtistValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertArtistAsync(It.IsAny<Artist>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async Task ShouldThrowValidationExceptionOnAddIfArtistIsInvalidAndLogItAsync(
            string invalidText)
        {
            //given
            var invalidArtist = new Artist
            {
                FirstName = invalidText,
                LastName = invalidText,
                Status = ArtistStatus.InActive
            };

            var invalidArtistException = new InvalidArtistException();

            invalidArtistException.AddData(
                key: nameof(Artist.Id),
                values: "Id is required.");

            invalidArtistException.AddData(
                key: nameof(Artist.FirstName),
                values: "Text is required.");

            invalidArtistException.AddData(
                key: nameof(Artist.LastName),
                values: "Text is required.");

            invalidArtistException.AddData(
                key: nameof(Artist.Status),
                values: "Value is invalid.");

            invalidArtistException.AddData(
               key: nameof(Artist.CreatedBy),
               values: "Id is required.");

            invalidArtistException.AddData(
               key: nameof(Artist.UpdatedBy),
               values: "Id is required.");

            invalidArtistException.AddData(
               key: nameof(Artist.CreatedDate),
               values: "Date is required.");

            invalidArtistException.AddData(
               key: nameof(Artist.UpdatedDate),
               values: "Date is required.");

            var expectedArtistValidationException =
               new ArtistValidationException(invalidArtistException);

            //when
            ValueTask<Artist> addArtistTask =
                this.artistService.AddArtistAsync(invalidArtist);

            //then
            await Assert.ThrowsAsync<ArtistValidationException>(() =>
                addArtistTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedArtistValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertArtistAsync(It.IsAny<Artist>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidEmails))]
        public async Task ShouldThrowValidationExceptionOnAddIfEmailIsInvalidAndLogItAsync(
            string invalidEmail)
        {
            //given
            Artist randomArtist = CreateRandomArtist();
            Artist invalidArtist = randomArtist;
            invalidArtist.Email = invalidEmail;
            var invalidArtistException = new InvalidArtistException();

            invalidArtistException.AddData(
                key: nameof(Artist.Email),
                values: "Text is invalid.");

            var expectedArtistValidationException =
               new ArtistValidationException(invalidArtistException);

            //when
            ValueTask<Artist> addArtistTask =
                this.artistService.AddArtistAsync(invalidArtist);

            //then
            await Assert.ThrowsAsync<ArtistValidationException>(() =>
                addArtistTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedArtistValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertArtistAsync(It.IsAny<Artist>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidContactNumbers))]
        public async Task ShouldThrowValidationExceptionOnAddIfContactNumberIsInvalidAndLogItAsync(
            string invalidContactNumber)
        {
            //given
            Artist randomArtist = CreateRandomArtist();
            Artist invalidArtist = randomArtist;
            invalidArtist.ContactNumber = invalidContactNumber;
            var invalidArtistException = new InvalidArtistException();

            invalidArtistException.AddData(
                key: nameof(Artist.ContactNumber),
                values: "Value is invalid.");

            var expectedArtistValidationException =
               new ArtistValidationException(invalidArtistException);

            //when
            ValueTask<Artist> addArtistTask =
                this.artistService.AddArtistAsync(invalidArtist);

            //then
            await Assert.ThrowsAsync<ArtistValidationException>(() =>
                addArtistTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedArtistValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertArtistAsync(It.IsAny<Artist>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedByNotSameAsUpdatedByAndLogItAsync()
        {
            // given
            Artist randomArtist = CreateRandomArtist();
            Artist invalidArtist = randomArtist;
            invalidArtist.CreatedBy = Guid.NewGuid();
            var invalidArtistException = new InvalidArtistException();

            invalidArtistException.AddData(
                key: nameof(Artist.CreatedBy),
                values: $"Id is not same as {nameof(Artist.UpdatedBy)}.");

            var expectedArtistValidationException =
                new ArtistValidationException(invalidArtistException);

            // when
            ValueTask<Artist> addArtistTask =
                this.artistService.AddArtistAsync(invalidArtist);

            // then
            await Assert.ThrowsAsync<ArtistValidationException>(() =>
                addArtistTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedArtistValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertArtistAsync(It.IsAny<Artist>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotSameAsUpdatedDateAndLogItAsync()
        {
            //given
            DateTimeOffset randomDate = GetRandomDateTime();
            Artist randomArtist = CreateRandomArtist(randomDate);
            Artist invalidArtist = randomArtist;
            invalidArtist.UpdatedDate = invalidArtist.CreatedDate.AddDays(1);
            var invalidArtistException = new InvalidArtistException();

            invalidArtistException.AddData(
                key: nameof(Artist.CreatedDate),
                values: $"Date is not same as {nameof(Artist.UpdatedDate)}.");

            var expectedArtistValidationException =
                new ArtistValidationException(invalidArtistException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            //when
            ValueTask<Artist> addArtistTask =
                this.artistService.AddArtistAsync(invalidArtist);

            //then
            await Assert.ThrowsAsync<ArtistValidationException>(() =>
                addArtistTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedArtistValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertArtistAsync(It.IsAny<Artist>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateDateIsNotRecentAndLogItAsync(
            int invalidMinuteCase)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Artist randomArtist = CreateRandomArtist(dateTime);
            Artist invalidArtist = randomArtist;

            invalidArtist.CreatedDate =
                invalidArtist.CreatedDate.AddMinutes(invalidMinuteCase);

            invalidArtist.UpdatedDate = invalidArtist.CreatedDate;

            var invalidArtistException =
                new InvalidArtistException();

            invalidArtistException.AddData(
                key: nameof(Artist.CreatedDate),
                values: "Date is not recent.");

            var expectedArtistValidationException =
                new ArtistValidationException(
                    invalidArtistException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Artist> addArtistTask =
                this.artistService.AddArtistAsync(invalidArtist);

            // then
            await Assert.ThrowsAsync<ArtistValidationException>(() =>
                addArtistTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedArtistValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertArtistAsync(It.IsAny<Artist>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
