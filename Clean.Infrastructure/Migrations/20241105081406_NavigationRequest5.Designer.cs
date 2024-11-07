﻿// <auto-generated />
using System;
using Clean.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Clean.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241105081406_NavigationRequest5")]
    partial class NavigationRequest5
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Clean.Domain.Entities.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPasswordExpire")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Clean.Domain.Entities.Approval", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AddedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("AddedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("ApprovalStatusId")
                        .HasColumnType("int");

                    b.Property<int>("ApproverId")
                        .HasColumnType("int");

                    b.Property<Guid>("GuidId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("RequestId")
                        .HasColumnType("int");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ApprovalStatusId");

                    b.HasIndex("ApproverId");

                    b.HasIndex("RequestId")
                        .IsUnique();

                    b.ToTable("Approvals");
                });

            modelBuilder.Entity("Clean.Domain.Entities.ApprovalStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ApprovalStatuses");
                });

            modelBuilder.Entity("Clean.Domain.Entities.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AddedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("AddedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("AppUserId")
                        .HasColumnType("int");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("nvarchar(21)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("GuidId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserRoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId")
                        .IsUnique();

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("UserRoleId");

                    b.ToTable("Employees");

                    b.HasDiscriminator().HasValue("Employee");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Clean.Domain.Entities.Request", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AddedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("AddedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("nvarchar(21)");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FromDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("GuidId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("RequestedBy")
                        .HasColumnType("int");

                    b.Property<int>("RequestedTo")
                        .HasColumnType("int");

                    b.Property<int>("RequestedTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ToDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("RequestedBy");

                    b.HasIndex("RequestedTo");

                    b.HasIndex("RequestedTypeId");

                    b.ToTable("Requests");

                    b.HasDiscriminator().HasValue("Request");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Clean.Domain.Entities.RequestedType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RequestedTypes");
                });

            modelBuilder.Entity("Clean.Domain.Entities.StoreProcedure.GetAllEmployees", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("GuidId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserRoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("GetAllEmployees");
                });

            modelBuilder.Entity("Clean.Domain.Entities.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Clean.Domain.Entities.View.EmployeeDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address_City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Address_PostalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Address_Street")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ApprovalId")
                        .HasColumnType("int");

                    b.Property<int?>("ApprovalStatusId")
                        .HasColumnType("int");

                    b.Property<int?>("ApproverId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("FromDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RequestId")
                        .HasColumnType("int");

                    b.Property<int?>("RequestedBy")
                        .HasColumnType("int");

                    b.Property<int?>("RequestedTo")
                        .HasColumnType("int");

                    b.Property<int?>("RequestedTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ToDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserRoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("EmployeeDetails");
                });

            modelBuilder.Entity("Clean.Domain.Entities.View.SqlInjection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("FromDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RequestId")
                        .HasColumnType("int");

                    b.Property<int?>("RequestedBy")
                        .HasColumnType("int");

                    b.Property<int?>("RequestedTo")
                        .HasColumnType("int");

                    b.Property<int?>("RequestedTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ToDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserRoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("SqlInjections");
                });

            modelBuilder.Entity("Clean.Infrastructure.OutBox.OutBoxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Errors")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("OccuredOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ProcessedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("OutBoxMessages");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Clean.Domain.Entities.GeneralEmployee", b =>
                {
                    b.HasBaseType("Clean.Domain.Entities.Employee");

                    b.HasDiscriminator().HasValue("GeneralEmployee");
                });

            modelBuilder.Entity("Clean.Domain.Entities.GeneralRequest", b =>
                {
                    b.HasBaseType("Clean.Domain.Entities.Request");

                    b.HasDiscriminator().HasValue("GeneralRequest");
                });

            modelBuilder.Entity("Clean.Domain.Entities.Approval", b =>
                {
                    b.HasOne("Clean.Domain.Entities.ApprovalStatus", "ApprovalStatus")
                        .WithMany()
                        .HasForeignKey("ApprovalStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Clean.Domain.Entities.Employee", "ApproverIdEmployee")
                        .WithMany()
                        .HasForeignKey("ApproverId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Clean.Domain.Entities.Request", "Request")
                        .WithOne("Approval")
                        .HasForeignKey("Clean.Domain.Entities.Approval", "RequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApprovalStatus");

                    b.Navigation("ApproverIdEmployee");

                    b.Navigation("Request");
                });

            modelBuilder.Entity("Clean.Domain.Entities.Employee", b =>
                {
                    b.HasOne("Clean.Domain.Entities.AppUser", "AppUser")
                        .WithOne()
                        .HasForeignKey("Clean.Domain.Entities.Employee", "AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Clean.Domain.Entities.UserRole", "UserRole")
                        .WithMany()
                        .HasForeignKey("UserRoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Clean.Domain.Entities.Address", "Address", b1 =>
                        {
                            b1.Property<int>("EmployeeId")
                                .HasColumnType("int");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(20)
                                .HasColumnType("nvarchar(20)");

                            b1.Property<bool>("IsDeleted")
                                .HasColumnType("bit");

                            b1.Property<string>("PostalCode")
                                .IsRequired()
                                .HasMaxLength(10)
                                .HasColumnType("nvarchar(10)");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasMaxLength(20)
                                .HasColumnType("nvarchar(20)");

                            b1.HasKey("EmployeeId");

                            b1.ToTable("Employees");

                            b1.WithOwner()
                                .HasForeignKey("EmployeeId");
                        });

                    b.Navigation("Address");

                    b.Navigation("AppUser");

                    b.Navigation("UserRole");
                });

            modelBuilder.Entity("Clean.Domain.Entities.Request", b =>
                {
                    b.HasOne("Clean.Domain.Entities.Employee", "Employee")
                        .WithMany("Requests")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Clean.Domain.Entities.Employee", "RequestedByEmployee")
                        .WithMany()
                        .HasForeignKey("RequestedBy")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Clean.Domain.Entities.Employee", "RequestedToEmployee")
                        .WithMany()
                        .HasForeignKey("RequestedTo")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Clean.Domain.Entities.RequestedType", "RequestedType")
                        .WithMany()
                        .HasForeignKey("RequestedTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("RequestedByEmployee");

                    b.Navigation("RequestedToEmployee");

                    b.Navigation("RequestedType");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Clean.Domain.Entities.UserRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("Clean.Domain.Entities.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("Clean.Domain.Entities.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("Clean.Domain.Entities.UserRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Clean.Domain.Entities.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("Clean.Domain.Entities.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Clean.Domain.Entities.Employee", b =>
                {
                    b.Navigation("Requests");
                });

            modelBuilder.Entity("Clean.Domain.Entities.Request", b =>
                {
                    b.Navigation("Approval");
                });
#pragma warning restore 612, 618
        }
    }
}
