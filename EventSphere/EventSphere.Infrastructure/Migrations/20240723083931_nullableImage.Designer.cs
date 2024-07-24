﻿// <auto-generated />
using System;
using EventSphere.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EventSphere.Infrastructure.Migrations
{
    [DbContext(typeof(EventSphereDbContext))]
    [Migration("20240723083931_nullableImage")]
    partial class nullableImage
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EventSphere.Domain.Entities.Event", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AvailableTickets")
                        .HasColumnType("int");

                    b.Property<int>("CategoryID")
                        .HasColumnType("int");

                    b.Property<string>("CategoryName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<int>("LocationId")
                        .HasColumnType("int");

                    b.Property<int>("MaxAttendance")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrganizerID")
                        .HasColumnType("int");

                    b.Property<string>("OrganizerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ScheduleDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("CategoryID");

                    b.HasIndex("LocationId");

                    b.HasIndex("OrganizerID");

                    b.ToTable("Event", (string)null);
                });

            modelBuilder.Entity("EventSphere.Domain.Entities.EventCategory", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID");

                    b.ToTable("EventCategory", (string)null);

                    b.HasData(
                        new
                        {
                            ID = 1,
                            CategoryName = "Concerts"
                        },
                        new
                        {
                            ID = 2,
                            CategoryName = "Sports"
                        },
                        new
                        {
                            ID = 3,
                            CategoryName = "Outside Activities"
                        });
                });

            modelBuilder.Entity("EventSphere.Domain.Entities.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Location", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            City = "Prishtina",
                            Country = "Kosovo",
                            Latitude = 42.6629,
                            Longitude = 21.165500000000002
                        },
                        new
                        {
                            Id = 2,
                            City = "Mitrovice",
                            Country = "Kosovo",
                            Latitude = 42.891399999999997,
                            Longitude = 20.866
                        },
                        new
                        {
                            Id = 3,
                            City = "Pejë",
                            Country = "Kosovo",
                            Latitude = 42.217100000000002,
                            Longitude = 20.743600000000001
                        },
                        new
                        {
                            Id = 4,
                            City = "Prizren",
                            Country = "Kosovo",
                            Latitude = 42.463500000000003,
                            Longitude = 21.4694
                        },
                        new
                        {
                            Id = 5,
                            City = "Ferizaj",
                            Country = "Kosovo",
                            Latitude = 42.370199999999997,
                            Longitude = 21.148299999999999
                        },
                        new
                        {
                            Id = 6,
                            City = "Gjilan",
                            Country = "Kosovo",
                            Latitude = 42.659300000000002,
                            Longitude = 20.288699999999999
                        },
                        new
                        {
                            Id = 7,
                            City = "Gjakovë",
                            Country = "Kosovo",
                            Latitude = 42.384399999999999,
                            Longitude = 20.4285
                        });
                });

            modelBuilder.Entity("EventSphere.Domain.Entities.Logg", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Exception")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Level")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MessageTemplate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Properties")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Logs", (string)null);
                });

            modelBuilder.Entity("EventSphere.Domain.Entities.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<bool>("IsRead")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications", (string)null);
                });

            modelBuilder.Entity("EventSphere.Domain.Entities.Payment", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("PaymentStatus")
                        .HasColumnType("bit");

                    b.Property<int>("TicketID")
                        .HasColumnType("int");

                    b.Property<string>("TicketName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("TicketID");

                    b.HasIndex("UserID");

                    b.ToTable("Payment", (string)null);
                });

            modelBuilder.Entity("EventSphere.Domain.Entities.PromoCode", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("DiscountPercentage")
                        .HasColumnType("float");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsValid")
                        .HasColumnType("bit");

                    b.HasKey("ID");

                    b.ToTable("PromoCodes");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Code = "B3LI3V3R",
                            DiscountPercentage = 20.0,
                            ExpiryDate = new DateTime(2024, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsValid = true
                        },
                        new
                        {
                            ID = 2,
                            Code = "FALLSALE",
                            DiscountPercentage = 15.0,
                            ExpiryDate = new DateTime(2024, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsValid = true
                        },
                        new
                        {
                            ID = 3,
                            Code = "SUMMERFUN",
                            DiscountPercentage = 10.0,
                            ExpiryDate = new DateTime(2024, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsValid = true
                        },
                        new
                        {
                            ID = 4,
                            Code = "SPRINGSALE",
                            DiscountPercentage = 15.0,
                            ExpiryDate = new DateTime(2024, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsValid = true
                        },
                        new
                        {
                            ID = 5,
                            Code = "WINTERWONDER",
                            DiscountPercentage = 25.0,
                            ExpiryDate = new DateTime(2024, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsValid = true
                        });
                });

            modelBuilder.Entity("EventSphere.Domain.Entities.Report", b =>
                {
                    b.Property<int>("reportId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("reportId"));

                    b.Property<string>("reportAnsw")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("reportDesc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("reportName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("userEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.Property<string>("userName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("userlastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("reportId");

                    b.ToTable("Report", (string)null);
                });

            modelBuilder.Entity("EventSphere.Domain.Entities.Role", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("ID");

                    b.ToTable("Role", (string)null);

                    b.HasData(
                        new
                        {
                            ID = 1,
                            RoleName = "Admin"
                        },
                        new
                        {
                            ID = 2,
                            RoleName = "Organizer"
                        },
                        new
                        {
                            ID = 3,
                            RoleName = "User"
                        });
                });

            modelBuilder.Entity("EventSphere.Domain.Entities.Ticket", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("BookingReference")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("EventID")
                        .HasColumnType("int");

                    b.Property<string>("EventName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("TicketAmount")
                        .HasColumnType("int");

                    b.Property<string>("TicketType")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("ID");

                    b.HasIndex("EventID");

                    b.ToTable("Ticket", (string)null);
                });

            modelBuilder.Entity("EventSphere.Domain.Entities.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<byte[]>("Password")
                        .IsRequired()
                        .HasColumnType("varbinary(64)");

                    b.Property<int>("RoleID")
                        .HasColumnType("int");

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Salt")
                        .IsRequired()
                        .HasColumnType("varbinary(64)");

                    b.HasKey("ID");

                    b.HasIndex("RoleID");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("EventSphere.Domain.Entities.Event", b =>
                {
                    b.HasOne("EventSphere.Domain.Entities.EventCategory", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EventSphere.Domain.Entities.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EventSphere.Domain.Entities.User", "Organizer")
                        .WithMany()
                        .HasForeignKey("OrganizerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Location");

                    b.Navigation("Organizer");
                });

            modelBuilder.Entity("EventSphere.Domain.Entities.Notification", b =>
                {
                    b.HasOne("EventSphere.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("EventSphere.Domain.Entities.Payment", b =>
                {
                    b.HasOne("EventSphere.Domain.Entities.Ticket", "Ticket")
                        .WithMany("Payments")
                        .HasForeignKey("TicketID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EventSphere.Domain.Entities.User", "User")
                        .WithMany("Payments")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ticket");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EventSphere.Domain.Entities.Ticket", b =>
                {
                    b.HasOne("EventSphere.Domain.Entities.Event", "Event")
                        .WithMany("Tickets")
                        .HasForeignKey("EventID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");
                });

            modelBuilder.Entity("EventSphere.Domain.Entities.User", b =>
                {
                    b.HasOne("EventSphere.Domain.Entities.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("EventSphere.Domain.Entities.Event", b =>
                {
                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("EventSphere.Domain.Entities.Ticket", b =>
                {
                    b.Navigation("Payments");
                });

            modelBuilder.Entity("EventSphere.Domain.Entities.User", b =>
                {
                    b.Navigation("Payments");
                });
#pragma warning restore 612, 618
        }
    }
}
