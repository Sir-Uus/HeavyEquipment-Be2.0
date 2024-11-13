using Application.Dtos;
using Application.Vm;
using AutoMapper;
using Domain.Entities;
// using Persistence.Migrations;

namespace Application.Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Equipment, Equipment>();
            CreateMap<EquipmentDto, Equipment>();
            CreateMap<FeedbackDto, Domain.Entities.PerformanceFeedback>();
            CreateMap<MaintenanceRecordDto, MaintenancedRecord>();
            CreateMap<RentalHistoryDto, RentalHistory>();
            CreateMap<SparePartsDto, SparePart>();
            CreateMap<RentalRequestDto, RentalRequest>();
            CreateMap<PaymentDto, Payment>();
            CreateMap<ImageDto, Images>();
            CreateMap<SparePartImageDto, SparePartImage>();
            CreateMap<TransactionDto, Transaction>();
            CreateMap<TransactionDetailsDto, TransactionDetail>();
            CreateMap<MessageDto, Message>();
            CreateMap<SparePartFeedbackDto, SparePartFeedback>();

            CreateMap<Equipment, EquipmentVm>();
            CreateMap<Equipment, EquipmentSummaryDto>();
            CreateMap<Domain.Entities.PerformanceFeedback, FeedbackVm>();
            CreateMap<MaintenancedRecord, MaintenanceRecordVm>();
            CreateMap<RentalHistory, RentalHistoryVm>();
            CreateMap<SparePart, SparePartVm>();
            CreateMap<RentalRequest, RentalRequestVm>();
            CreateMap<RentalRequest, RentalRequestDto>();
            CreateMap<Payment, PaymentVm>();
            CreateMap<Payment, PaymentDto>();
            CreateMap<User, UserSummaryDto>();
            CreateMap<Transaction, TransactionVm>();
            CreateMap<Transaction, TransactionDto>();
            CreateMap<TransactionDetail, TransactionDetailsVm>();
            CreateMap<Message, MessageVm>();
            CreateMap<SparePartFeedback, SparePartFeedbackVm>();
            CreateMap<SparePart, SparePartsDto>();
            CreateMap<Images, ImageDetailsVm>();
            CreateMap<Transaction, TransactionSummaryDetailsVm>();
            CreateMap<SparePartImage, SparePartImageDetailsVm>();
            CreateMap<SparePartImage, SparePartImagesVm>()
                .ForMember(dst => dst.ImageId, opt => opt.MapFrom(src => src.Id));
            CreateMap<Images, ImagesVm>()
                .ForMember(dst => dst.ImageId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
