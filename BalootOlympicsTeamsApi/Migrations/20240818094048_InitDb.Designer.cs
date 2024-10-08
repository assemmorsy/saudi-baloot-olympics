﻿// <auto-generated />
using System;
using BalootOlympicsTeamsApi.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BalootOlympicsTeamsApi.Migrations
{
    [DbContext(typeof(OlympicsContext))]
    [Migration("20240818094048_InitDb")]
    partial class InitDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "uuid-ossp");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BalootOlympicsTeamsApi.Entities.ConfirmationRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTimeOffset>("FirstPlayerConfirmationSentAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("first_player_confirmation_sent_at");

                    b.Property<DateTimeOffset?>("FirstPlayerConfirmedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("first_player_confirmed_at");

                    b.Property<string>("FirstPlayerNationalId")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("first_player_national_id");

                    b.Property<string>("FirstPlayerOtp")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("character varying(6)")
                        .HasColumnName("first_player_otp");

                    b.Property<DateTimeOffset?>("SecondPlayerConfirmationSentAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("second_player_confirmation_sent_at");

                    b.Property<DateTimeOffset?>("SecondPlayerConfirmedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("second_player_confirmed_at");

                    b.Property<string>("SecondPlayerNationalId")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("second_player_national_id");

                    b.Property<string>("SecondPlayerOtp")
                        .HasMaxLength(6)
                        .HasColumnType("character varying(6)")
                        .HasColumnName("second_player_otp");

                    b.HasKey("Id")
                        .HasName("confirmation_request_pkey");

                    b.HasIndex("FirstPlayerNationalId");

                    b.HasIndex("SecondPlayerNationalId");

                    b.ToTable("confirmation_request", (string)null);
                });

            modelBuilder.Entity("BalootOlympicsTeamsApi.Entities.Player", b =>
                {
                    b.Property<string>("NationalId")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("national_id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("email");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("phone");

                    b.HasKey("NationalId")
                        .HasName("players_pkey");

                    b.HasIndex(new[] { "Email" }, "player_email_key")
                        .IsUnique();

                    b.HasIndex(new[] { "Phone" }, "player_phone_key")
                        .IsUnique();

                    b.ToTable("players", (string)null);
                });

            modelBuilder.Entity("BalootOlympicsTeamsApi.Entities.ConfirmationRequest", b =>
                {
                    b.HasOne("BalootOlympicsTeamsApi.Entities.Player", "FirstPlayer")
                        .WithMany()
                        .HasForeignKey("FirstPlayerNationalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_first_player_id");

                    b.HasOne("BalootOlympicsTeamsApi.Entities.Player", "SecondPlayer")
                        .WithMany()
                        .HasForeignKey("SecondPlayerNationalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("fk_second_player_id");

                    b.Navigation("FirstPlayer");

                    b.Navigation("SecondPlayer");
                });
#pragma warning restore 612, 618
        }
    }
}
