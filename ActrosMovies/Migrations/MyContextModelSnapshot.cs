﻿// <auto-generated />
using System;
using ActrosMovies.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ActrosMovies.Migrations
{
    [DbContext(typeof(MyContext))]
    partial class MyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ActrosMovies.Models.Actor", b =>
                {
                    b.Property<int>("ActorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ActorId");

                    b.HasIndex("UserId");

                    b.ToTable("Actors");
                });

            modelBuilder.Entity("ActrosMovies.Models.Association", b =>
                {
                    b.Property<int>("AssociationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ActorId")
                        .HasColumnType("int");

                    b.Property<int>("MovieId")
                        .HasColumnType("int");

                    b.HasKey("AssociationId");

                    b.HasIndex("ActorId");

                    b.HasIndex("MovieId");

                    b.ToTable("Associations");
                });

            modelBuilder.Entity("ActrosMovies.Models.Movie", b =>
                {
                    b.Property<int>("MovieId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("MovieImage")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("MovieId");

                    b.HasIndex("UserId");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("ActrosMovies.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ActrosMovies.Models.Actor", b =>
                {
                    b.HasOne("ActrosMovies.Models.User", null)
                        .WithMany("CreatedActors")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("ActrosMovies.Models.Association", b =>
                {
                    b.HasOne("ActrosMovies.Models.Actor", "Actor")
                        .WithMany("AllMovies")
                        .HasForeignKey("ActorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ActrosMovies.Models.Movie", "Movie")
                        .WithMany("AllActors")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Actor");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("ActrosMovies.Models.Movie", b =>
                {
                    b.HasOne("ActrosMovies.Models.User", null)
                        .WithMany("CreatedMovies")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("ActrosMovies.Models.Actor", b =>
                {
                    b.Navigation("AllMovies");
                });

            modelBuilder.Entity("ActrosMovies.Models.Movie", b =>
                {
                    b.Navigation("AllActors");
                });

            modelBuilder.Entity("ActrosMovies.Models.User", b =>
                {
                    b.Navigation("CreatedActors");

                    b.Navigation("CreatedMovies");
                });
#pragma warning restore 612, 618
        }
    }
}
